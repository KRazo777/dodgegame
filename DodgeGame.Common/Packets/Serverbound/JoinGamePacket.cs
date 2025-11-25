using System;
using System.Linq;
using System.Numerics;
using DodgeGame.Common.Game;
using DodgeGame.Common.Packets.Clientbound;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class JoinGamePacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.JoinGame;

        private string _roomId;
        
        public string RoomId => _roomId;

        public JoinGamePacket()
        {
        }

        public JoinGamePacket(string roomId)
        {
            _roomId = roomId;
        }

        public override void Deserialize(Message message)
        {
            _roomId = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(_roomId);
            message.AddString(""); //room password
            return message;
        }

        public void Process(IGameServer gameServer, Client client)
        {
            var room = gameServer.GameRooms[_roomId];
            if (room.Players.Count >= 4)
            {
                return;
            }

            if (room.IsStarted)
            {
                return;
            }
            
            //TODO: handle private lobbies
            
            Console.WriteLine("Client username " + client.User.Username + " joined room " + room.RoomId);
            var player = new Player(client.User.UniqueId, client.User.Username, EntityType.Player);
            Console.WriteLine("Player " + player.Id + " created");
            Console.WriteLine(player.Name);
            room.Players.TryAdd(player.Id, player);
            client.User.Player = player;
            client.User.Player.GameRoom = room;
            
            client.SendPacket(new JoinedGamePacket(room, player));
            foreach (var player1 in room.Players.Values.Where(p => !p.Id.Equals(player.Id)))
            {
                var playerClient = gameServer.GetClient(player1.Id);
                if (playerClient != null) playerClient.SendPacket(new PlayerAddedPacket(room.RoomId, player));
            }

            if (room.Players.Count >= 2)
            {
                foreach (var uuid in room.Players.Keys)
                {
                    var playerClient = gameServer.GetClient(uuid);
                    if (playerClient != null)
                    {
                        room.Players[uuid].Position = room.SpawnManager.GetUniqueSpawnPoint() ?? new Vector2();
                    }
                }
                
                foreach (var uuid in room.Players.Keys)
                {
                    var playerClient = gameServer.GetClient(uuid);
                    if (playerClient != null)
                    {
                        Console.WriteLine("Starting game");
                        playerClient.SendPacket(new StartGamePacket(room));
                    }
                }
            }
            
        }
    }
}