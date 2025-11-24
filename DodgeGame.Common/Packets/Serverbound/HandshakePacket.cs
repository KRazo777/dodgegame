using DodgeGame.Common;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class HandshakePacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.Handshake;
        public string UniqueId { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public long DateCreated { get; private set; }

        public HandshakePacket()
        {
        }

        public HandshakePacket(string uniqueId, string username, long dateCreated)
        {
            UniqueId = uniqueId;
            Username = username;
            DateCreated = dateCreated;
        }

        public override void Deserialize(Message message)
        {
            UniqueId = message.GetString();
            Username = message.GetString();
            DateCreated = message.GetLong();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.Handshake);
            message.AddString(UniqueId);
            message.AddString(Username);
            message.AddLong(DateCreated);
            return message;
        }

        public override void Process(Client client)
        {
            
        }
    }
}
