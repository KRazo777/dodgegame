using System;
using System.Numerics;
using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class BulletFiredPacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.BulletFired;

        public string OwnerId { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        
        public float RotationX { get; private set; }
        public float RotationY { get; private set; }
        public float RotationZ { get; private set; }
        public float RotationW { get; private set; }

        public BulletFiredPacket()
        {
        }

        public BulletFiredPacket(string ownerId, float x, float y, float rotationX, float rotationY, float rotationZ, float rotationW)
        {
            OwnerId = ownerId;
            X = x;
            Y = y;
            RotationX = rotationX;
            RotationY = rotationY;
            RotationZ = rotationZ;
            RotationW = rotationW;
        }

        public override void Deserialize(Message message)
        {
            OwnerId = message.GetString();
            X = message.GetFloat();
            Y = message.GetFloat();
            RotationX = message.GetFloat();
            RotationY = message.GetFloat();
            RotationZ = message.GetFloat();
            RotationW = message.GetFloat();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(OwnerId);
            message.AddFloat(X);
            message.AddFloat(Y);
            message.AddFloat(RotationX);
            message.AddFloat(RotationY);
            message.AddFloat(RotationZ);
            message.AddFloat(RotationW);
            return message;
        }

        public void Process(IGameServer server, Client client)
        {
            var room = client.User?.Player?.GameRoom;
            if (room == null) return;
            
            var uid = Guid.NewGuid().ToString();
            var bullet = new Bullet(uid, OwnerId, EntityType.Bullet);
            bullet.Position = new Vector2(X, Y);
            bullet.Rotation = new Vector3(RotationX, RotationY, RotationZ);
            room.Entities[uid] = bullet;
            
            
            foreach (var playersValue in room.Players.Values)
            {
                var playerClient = server.GetClient(playersValue.Id);
                if (playerClient != null) playerClient.SendPacket(new Clientbound.BulletFiredPacket(
                    OwnerId, X, Y, RotationX, RotationY, RotationZ, RotationW, uid));
            }
        }
    }
}