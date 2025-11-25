using System;
using DodgeGame.Common.Packets.Clientbound;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class BulletHitPacket : Packet , IServerPacket
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
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(HitPlayerUniqueId);
            message.AddString(BulletOwnerUniqueId);
            return message;
        }

        public void Process(IGameServer server, Client client)
        {
            var ownerClient = server.GetClient(BulletOwnerUniqueId);

            var room = ownerClient?.User?.Player?.GameRoom;
            if (room == null) return;

            // We only want to believe it when the host sends it
            if (client.User.UniqueId != room.HostUniqueId) return;
            
            Console.WriteLine("Bullet hit " + HitPlayerUniqueId);

            room.Players[HitPlayerUniqueId].IsAlive = false;
            room.Players[BulletOwnerUniqueId].Kills++;
            
            foreach (var playersValue in room.Players.Values)
            {
                var playerClient = server.GetClient(playersValue.Id);
                if (playerClient != null) playerClient.SendPacket(new PlayerDeathPacket(HitPlayerUniqueId, BulletOwnerUniqueId));
            }
        }
    }
}