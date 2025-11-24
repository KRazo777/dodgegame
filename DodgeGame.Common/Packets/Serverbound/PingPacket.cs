using DodgeGame.Common.Packets.Clientbound;
using Riptide;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class PingPacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.Ping;
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
            SentAtTicks = message.GetLong();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddLong(SentAtTicks);
            return message;
        }

        public void Process(IGameServer gameServer, Manager.Client client)
        {
            client.SendPacket(new PongPacket(SentAtTicks));
        }
    }
}
