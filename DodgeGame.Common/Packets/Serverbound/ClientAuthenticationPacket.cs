using DodgeGame.Common;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class ClientAuthenticationPacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.ClientAuth;

        private string _token;
        
        public string Token => _token;

        public ClientAuthenticationPacket()
        {
        }
        
        public ClientAuthenticationPacket(string token)
        {
            _token = token;
        }

        public override void Deserialize(Message message)
        {
            _token = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.ClientAuth);
            message.AddString(_token);
            return message;
        }

        public override void Process(Client client)
        {
        }
    }
}
