using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class AcknowledgementPacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.Acknowledgement;
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
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.Acknowledgement);
            message.AddUShort(SequenceId);
            message.AddUShort(PacketId);
            message.AddUShort(AcknowledgedSequenceId);
            return message;
        }

        public override void Process(Client client)
        {
            // Record that the client confirmed receipt of the server's packet.
            client.RegisterClientboundAcknowledgement(AcknowledgedSequenceId);
        }
    }
}
