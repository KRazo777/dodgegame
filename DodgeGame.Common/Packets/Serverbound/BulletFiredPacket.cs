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
        
        // We only need Z angle for 2D
        public float RotationZ { get; private set; } 

        public BulletFiredPacket() {}

        public BulletFiredPacket(string ownerId, float x, float y, float rotationZ)
        {
            OwnerId = ownerId;
            X = x;
            Y = y;
            RotationZ = rotationZ;
        }

        public override void Deserialize(Message message)
        {
            OwnerId = message.GetString();
            X = message.GetFloat();
            Y = message.GetFloat();
            RotationZ = message.GetFloat();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(OwnerId);
            message.AddFloat(X);
            message.AddFloat(Y);
            message.AddFloat(RotationZ);
            return message;
        }

        public void Process(IGameServer server, Client client)
        {
            var room = client.User?.Player?.GameRoom;
            if (room == null) return;
            
            var uid = Guid.NewGuid().ToString();
            
            var bullet = new Bullet(uid, OwnerId, EntityType.Bullet);
            bullet.Position = new Vector2(X, Y);
            
            bullet.Rotation = new Vector3(0, 0, RotationZ); 
            
            room.Entities[uid] = bullet;
            
            // Broadcast to other players
            foreach (var playersValue in room.Players.Values)
            {
                var playerClient = server.GetClient(playersValue.Id);
                
                if (playerClient != null) 
                {
                    playerClient.SendPacket(new Clientbound.BulletFiredPacket(
                        OwnerId, X, Y, RotationZ, uid
                    ));
                }
            }
        }
    }
}