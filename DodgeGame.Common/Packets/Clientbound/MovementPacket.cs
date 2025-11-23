using Riptide;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class MovementPacket : Packet
    {
        public override ushort Id => PacketIds.Clientbound.Movement;

        public string UniqueId { get; private set; } = string.Empty;
        public float X { get; private set; }
        public float Y { get; private set; }

        public MovementPacket()
        {
        }

        public MovementPacket(string uniqueId, float x, float y)
        {
            UniqueId = uniqueId;
            X = x;
            Y = y;
        }

        public override void Deserialize(Message message)
        {
            UniqueId = message.GetString();
            X = message.GetFloat();
            Y = message.GetFloat();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Unreliable, PacketIds.Clientbound.Movement);
            message.AddString(UniqueId);
            message.AddFloat(X);
            message.AddFloat(Y);
            return message;
        }

        public override void Process(Manager.Client client)
        {
            // Clientbound packet: handled client-side to update entity positions.
        }
    }
}
