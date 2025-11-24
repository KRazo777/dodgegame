using Riptide;

namespace DodgeGame.Common.Packets.Clientbound
{
    using Client = DodgeGame.Common.Manager.Client;
    public class HandshakePacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.Handshake;
        public ushort ClientId { get; private set; }
        public string UniqueId { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public long DateCreated { get; private set; }

        public HandshakePacket()
        {
        }

        public HandshakePacket(ushort clientId, string uniqueId, string username, long dateCreated)
        {
            ClientId = clientId;
            UniqueId = uniqueId;
            Username = username;
            DateCreated = dateCreated;
        }

        public override void Deserialize(Message message)
        {
            ClientId = message.GetUShort();
            UniqueId = message.GetString();
            Username = message.GetString();
            DateCreated = message.GetLong();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddUShort(ClientId);
            message.AddString(UniqueId);
            message.AddString(Username);
            message.AddLong(DateCreated);
            return message;
        }

        public void Process(Client client)
        {
            // Clientbound packets are not processed on the server.
        }
    }
}