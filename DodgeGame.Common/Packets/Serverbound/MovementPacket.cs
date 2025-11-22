using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class MovementPacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.Movement;

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
            SequenceId = message.GetUShort();
            UniqueId = message.GetString();
            X = message.GetFloat();
            Y = message.GetFloat();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Unreliable, PacketIds.Serverbound.Movement);
            message.AddUShort(SequenceId);
            message.AddString(UniqueId);
            message.AddFloat(X);
            message.AddFloat(Y);
            return message;
        }

        public override void Process(Client client)
        {
            // Server should update the tracked position for this client.
        }
    }
}
