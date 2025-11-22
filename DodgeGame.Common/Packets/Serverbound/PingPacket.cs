using Riptide;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class PingPacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.Ping;
        public long SentAtTicks { get; private set; }

        public PingPacket()
        {
        }

        public PingPacket(long sentAtTicks)
        {
            SentAtTicks = sentAtTicks;
        }

        public override void Deserialize(Message message)
        {
            SequenceId = message.GetUShort();
            SentAtTicks = message.GetLong();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.Ping);
            message.AddUShort(SequenceId);
            message.AddLong(SentAtTicks);
            return message;
        }

        public override void Process(Manager.Client client)
        {
            // Clientbound packets are not processed on the server.
        }
    }
}
