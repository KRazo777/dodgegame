using System.Numerics;
using Newtonsoft.Json;
using Riptide;

namespace DodgeGame.Common.Game
{
    public class Player : Entity, ISerializable<Player>
    {
        public readonly string Name;
        public byte Kills { get; set; }
        public ushort BulletCount { get; set; }
        public bool IsAlive { get; set; }
        public byte LivesRemaining { get; set; }
        
        [JsonIgnore]
        public GameRoom? GameRoom { get; set; }
        
        public Player(string uniqueId, string name, EntityType entityType) : base(uniqueId, entityType)
        {
            Name = name ?? string.Empty;
        }
    
        public void IncKill() => Kills++;
        public void IncBulletCount() => BulletCount++;
        public void DecLivesRemaining() => LivesRemaining--;
    
        public void SetDead() => IsAlive = false;
        public void Serialize(Message message)
        {
            message.AddString(Id);
            message.AddString(Name ?? string.Empty);
            message.AddByte(Kills);
            message.AddUShort(BulletCount);
            message.AddBool(IsAlive);
            message.AddByte(LivesRemaining);
            message.AddFloat(Position.X);
            message.AddFloat(Position.Y);
        }

        public static Player Deserialize(Message message)
        {
            var player = new Player(message.GetString(), message.GetString(), EntityType.Player)
            {
                Kills = message.GetByte(),
                BulletCount = message.GetUShort(),
                IsAlive = message.GetBool(),
                LivesRemaining = message.GetByte(),
                Position = new Vector2(message.GetFloat(), message.GetFloat())
            };
            return player;
        }
    }
}
