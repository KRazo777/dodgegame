using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PongPacket : Packet
    {
        public override ushort Id => PacketIds.Clientbound.Pong;
        public long SentAtTicks { get; private set; }

        public PongPacket()
        {
        }

        public PongPacket(long sentAtTicks)
        {
            SentAtTicks = sentAtTicks;
        }

        public override void Deserialize(Message message)
        {
            SentAtTicks = message.GetLong();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.Pong);
            message.AddLong(SentAtTicks);
            return message;
        }

        public override void Process(Client client)
        {
            // Server removes pending ping on receipt elsewhere; store latency info as needed.
        }
    }
}
