using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class BulletHitPacket : Packet
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.BulletHit;

        public string HitPlayerUniqueId { get; private set; } = string.Empty;
        public string BulletOwnerUniqueId { get; private set; } = string.Empty;

        public BulletHitPacket() {}

        public BulletHitPacket(string hitId, string ownerId)
        {
            HitPlayerUniqueId = hitId;
            BulletOwnerUniqueId = ownerId;
        }

        public override void Deserialize(Message message)
        {
            HitPlayerUniqueId = message.GetString();
            BulletOwnerUniqueId = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.BulletHit);
            message.AddString(HitPlayerUniqueId);
            message.AddString(BulletOwnerUniqueId);
            return message;
        }

        public void Process(Client client)
        {
            // Server logic will handle this to validate the hit and announce EndRound
        }
    }
}