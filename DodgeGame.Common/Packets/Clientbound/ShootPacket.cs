using System.Numerics;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class ShootPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.Shoot;

        public string UniqueId { get; private set; } = string.Empty;
        public float startX { get; private set; }
        public float startY { get; private set; }
        public float velocityX { get; private set; }
        public float velocityY { get; private set; }

        public ShootPacket()
        {
        }

        public ShootPacket(string uniqueId, float x, float y, float velX, float velY)
        {
            UniqueId = uniqueId;
            startX = x;
            startY = y;
            velocityX = velX;
            velocityY = velY;
        }

        public override void Deserialize(Message message)
        {
            UniqueId = message.GetString();
            startX = message.GetFloat();
            startY = message.GetFloat();
            velocityX = message.GetFloat();
            velocityY = message.GetFloat();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Unreliable, Id);
            message.AddString(UniqueId);
            message.AddFloat(startX);
            message.AddFloat(startY);
            message.AddFloat(velocityX);
            message.AddFloat(velocityY);
            return message;
        }

        public void Process(Client client)
        {
            // CLIENT SIDE CONNECTION HANDLER SHOULD INSTANTIATE A BULLET
        }
    }
}