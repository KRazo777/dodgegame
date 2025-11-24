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

        public GameListPacket()
        {
        }

        public GameListPacket(GameRoom[] gameRooms)
        {
            _gameRooms = gameRooms;
        }

        public override void Deserialize(Message message)
        {
            var length = message.GetUInt();
            _gameRooms = new GameRoom[length];
            for (var i = 0; i < length; i++)
            {
                var room = new GameRoom(
                    message.GetString(),
                    message.GetString(),
                    message.GetString());
                for (var players = 0; players < message.GetByte(); players++)
                {
                    // placeholder - don't want to send all player data
                    room.Players.Add(Guid.NewGuid().ToString(), new Player("", "", EntityType.Player));
                }

                _gameRooms[i] = room;
            }
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddUInt((uint)_gameRooms.Length);
            foreach (var gameRoom in _gameRooms.Where(gr => !gr.IsPrivate))
            {
                message.AddString(gameRoom.HostUniqueId);
                message.AddString(gameRoom.RoomId);
                message.AddString(gameRoom.OwnerName);
                message.AddByte((byte)gameRoom.Players.Count);
            }

            return message;
        }

        public void Process(Client client)
        {
        }
    }
}