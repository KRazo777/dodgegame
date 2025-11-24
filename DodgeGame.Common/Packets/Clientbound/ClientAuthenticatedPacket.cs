using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class ClientAuthenticatedPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.ClientAuth;

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
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(_user.UniqueId);
            message.AddString(_user.Username);
            message.AddLong(_user.DateCreated);
            return message;
        }

        public void Process(Client client)
        {
            client.User = _user;
        }
    }
}