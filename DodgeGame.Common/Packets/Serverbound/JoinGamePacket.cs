using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class JoinGamePacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.GameJoin;

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
            return message;
        }

        public void Process(IGameServer gameServer, Client client)
        {
        }
    }
}