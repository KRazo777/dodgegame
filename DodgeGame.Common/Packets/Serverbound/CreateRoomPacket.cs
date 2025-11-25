using DodgeGame.Common.Game;
using DodgeGame.Common.Packets.Clientbound;
using DodgeGame.Common.Util;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class CreateRoomPacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.CreateRoom;

        private string _roomPassword;
        private bool _isPrivate;

        public string RoomPassword => _roomPassword;
        public bool IsPrivate => _isPrivate;

        public CreateRoomPacket()
        {
        }

        public CreateRoomPacket(string roomPassword = "", bool isPrivate = false)
        {
            _roomPassword = roomPassword;
            _isPrivate = isPrivate;
        }

        public override void Deserialize(Message message)
        {
            _roomPassword = message.GetString();
            _isPrivate = message.GetBool();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(_roomPassword);
            message.AddBool(_isPrivate);
            return message;
        }

        public void Process(IGameServer gameServer, Client client)
        {
            var room = new GameRoom(client.User.UniqueId, StringGenerator.GenerateAlpha6(), client.User.Username)
            {
                RoomPassword = _roomPassword.Trim(),
                IsPrivate = _isPrivate
            };
            gameServer.GameRooms.TryAdd(room.RoomId, room);

            var player = new Player(client.User.UniqueId, client.User.Username, EntityType.Player);
            client.User.Player = player;
            client.User.Player.GameRoom = room;

            room.Players.TryAdd(player.Id, player);

            client.SendPacket(new CreatedRoomPacket(room, player));
            
        }
    }
}