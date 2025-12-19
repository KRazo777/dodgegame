using DodgeGame.Common.Game;
using DodgeGame.Common.Manager;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class BulletFiredPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.BulletFired;

        public string OwnerId { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float RotationZ { get; private set; }
        public string Uid { get; private set; }

        public BulletFiredPacket()
        {
        }

        public BulletFiredPacket(string ownerId, float x, float y, float rotationZ, string uid)
        {
            OwnerId = ownerId;
            X = x;
            Y = y;
            RotationZ = rotationZ;
            Uid = uid;
        }

        public override void Deserialize(Message message)
        {
            OwnerId = message.GetString();
            X = message.GetFloat();
            Y = message.GetFloat();
            RotationZ = message.GetFloat();
            Uid = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(OwnerId);
            message.AddFloat(X);
            message.AddFloat(Y);
            message.AddFloat(RotationZ);
            message.AddString(Uid);
            return message;
        }

        // ✅ FIX: Added the Process method required by the interface
        public void Process(Client client)
        {
            // We leave this empty because ConnectionHandler handles the logic manually!
        }
    }
}