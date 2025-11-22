using Riptide;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class AcknowledgementPacket : Packet
    {
        public override ushort Id => PacketIds.Clientbound.Acknowledgement;
        public ushort PacketId { get; private set; }
        public ushort AcknowledgedSequenceId { get; set; }

        public AcknowledgementPacket()
        {
        }

        public AcknowledgementPacket(ushort packetId)
        {
            PacketId = packetId;
        }

        public override void Deserialize(Message message)
        {
            SequenceId = message.GetUShort();
            PacketId = message.GetUShort();
            AcknowledgedSequenceId = message.GetUShort();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.Acknowledgement);
            message.AddUShort(SequenceId);
            message.AddUShort(PacketId);
            message.AddUShort(AcknowledgedSequenceId);
            return message;
        }

        public override void Process(Manager.Client client)
        {
            // Clientbound packets are not processed on the server.
        }
    }
}
