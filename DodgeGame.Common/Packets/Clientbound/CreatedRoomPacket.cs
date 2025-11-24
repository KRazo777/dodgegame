using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class CreatedRoomPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.CreatedRoom;

        private GameRoom _gameRoom;
        private Player _hostPlayer;

        public Player HostPlayer => _hostPlayer;
        public GameRoom GameRoom => _gameRoom;

        public CreatedRoomPacket()
        {
        }

        public CreatedRoomPacket(GameRoom gameRoom, Player hostPlayer)
        {
            _gameRoom = gameRoom;
            _hostPlayer = hostPlayer;
        }

        public override void Deserialize(Message message)
        {
            _gameRoom = new GameRoom(
                message.GetString(),
                message.GetString(),
                message.GetString())
            {
                RoomPassword = message.GetString(),
                IsPrivate = message.GetBool()
            };

            _hostPlayer = new Player(message.GetString(), message.GetString(), EntityType.Player);
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(_gameRoom.HostUniqueId);
            message.AddString(_gameRoom.RoomId);
            message.AddString(_gameRoom.OwnerName);
            message.AddString(_gameRoom.RoomPassword);
            message.AddBool(_gameRoom.IsPrivate);
            message.AddString(_hostPlayer.Id);
            message.AddString(_hostPlayer.Name);
            return message;
        }

        public void Process(Client client)
        {
            client.User.Player = _hostPlayer;
            client.User.Player.GameRoom = _gameRoom;
        }
    }
}