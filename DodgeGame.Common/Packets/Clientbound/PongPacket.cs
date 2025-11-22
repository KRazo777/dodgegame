using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PongPacket : Packet
    {
        public override ushort Id => PacketIds.Clientbound.Pong;
        public ushort PingSequenceId { get; private set; }
        public long SentAtTicks { get; private set; }

        public PongPacket()
        {
        }

        public PongPacket(ushort pingSequenceId, long sentAtTicks)
        {
            PingSequenceId = pingSequenceId;
            SentAtTicks = sentAtTicks;
        }

        public override void Deserialize(Message message)
        {
            SequenceId = message.GetUShort();
            PingSequenceId = message.GetUShort();
            SentAtTicks = message.GetLong();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.Pong);
            message.AddUShort(SequenceId);
            message.AddUShort(PingSequenceId);
            message.AddLong(SentAtTicks);
            return message;
        }

        public override void Process(Client client)
        {
            // Server removes pending ping on receipt elsewhere; store latency info as needed.
        }
    }
}
