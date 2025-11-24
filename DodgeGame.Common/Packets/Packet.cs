using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets
{
    public abstract class Packet
    {
        public abstract ushort Id { get; }
        public abstract void Deserialize(Message message);
        public abstract Message Serialize();
    }
}
