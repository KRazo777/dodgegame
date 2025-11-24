using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PongPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.Pong;
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
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddLong(SentAtTicks);
            return message;
        }

        public void Process(Client client)
        {
            // Server removes pending ping on receipt elsewhere; store latency info as needed.
        }
    }
}