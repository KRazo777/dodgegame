using Riptide;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PlayerQuitPacket : Packet
    {
        public override ushort Id => PacketIds.Clientbound.PlayerQuit; // Assign a new ID

        public string UniqueId { get; private set; } = string.Empty;

        public PlayerQuitPacket()
        {
        }

        public PlayerQuitPacket(string uniqueId)
        {
            UniqueId = uniqueId;
        }

        public override void Deserialize(Message message)
        {
            UniqueId = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.PlayerQuit);
            message.AddString(UniqueId);
            return message;
        }

        public override void Process(Manager.Client client)
        {
            // Remove the player from playercount and disconnect them from room
        }
    }
}