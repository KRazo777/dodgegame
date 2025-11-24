using DodgeGame.Common;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class ClientAuthenticatedPacket : Packet
    {
        public override ushort Id => PacketIds.Clientbound.ClientAuth;

        private User _user;

        public ClientAuthenticatedPacket()
        {
        }

        public ClientAuthenticatedPacket(User user)
        {
            _user = user;
        }

        public override void Deserialize(Message message)
        {
            _user = new User(
                message.GetString(),
                message.GetString(),
                message.GetLong()
                );
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.ClientAuth);
            message.AddString(_user.UniqueId);
            message.AddString(_user.Username);
            message.AddLong(_user.DateCreated);
            return message;
        }

        public override void Process(Client client)
        {
            client.User = _user;
        }
    }
}