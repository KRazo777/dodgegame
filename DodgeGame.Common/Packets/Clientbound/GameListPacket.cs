using System;
using System.Linq;
using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class GameListPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.GameList;

        private GameRoom[] _gameRooms;
        public GameRoom[] GameRooms => _gameRooms;

        public GameListPacket() { }
        public GameListPacket(GameRoom[] gameRooms) { _gameRooms = gameRooms; }

        public override void Deserialize(Message message)
        {
            try 
            {
                System.Console.WriteLine("[DEBUG] STARTING DESERIALIZE");

                if (message.UnreadLength == 0) {
                    _gameRooms = new GameRoom[0];
                    return;
                }

                var length = message.GetUInt();
                _gameRooms = new GameRoom[length];

                for (var i = 0; i < length; i++)
                {
                    string host = message.GetString();
                    string id = message.GetString();
                    string owner = message.GetString();

                    byte playerCount = message.GetByte();
                    System.Console.WriteLine($"[DEBUG] Room {id}: Player Count = {playerCount}");
                    
                    var room = new GameRoom(host, id, owner);
                    for (var p = 0; p < playerCount; p++)
                    {
                        room.Players.Add(Guid.NewGuid().ToString(), new Player("", "", EntityType.Player));
                    }
                    
                    _gameRooms[i] = room;
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("[DEBUG] CRASH IN DESERIALIZE!");
                throw;
            }
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            
            if (_gameRooms == null) { message.AddUInt(0); return message; }

            // Filter Public Rooms
            var publicRooms = _gameRooms.Where(gr => !gr.IsPrivate).ToArray();
            
            // WRITE COUNT
            message.AddUInt((uint)publicRooms.Length);

            foreach (var room in publicRooms)
            {
                message.AddString(room.HostUniqueId);
                message.AddString(room.RoomId);
                message.AddString(room.OwnerName);
                
                // WRITE PLAYERS
                message.AddByte((byte)room.Players.Count);
            }
            return message;
        }

        public void Process(Client client) { }
    }
}