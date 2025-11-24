using System;
using System.Linq;
using DodgeGame.Common.Packets.Clientbound;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class ClientAuthenticationPacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.ClientAuth;

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
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(_token);
            return message;
        }

        public void Process(IGameServer gameServer, Client client)
        {
            
        }
    }
}
