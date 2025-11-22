namespace DodgeGame.Common.Game
{
    public class Player : Entity
    {
        public readonly string Name;

        public Player(string uniqueId, string name, EntityType entityType) : base(uniqueId, entityType)
        {
            Name = name;
        }

        public byte Kills { get; set; }
        public ushort BulletCount { get; set; }
        public bool IsAlive { get; set; }
        public byte LivesRemaining { get; set; }
        
        public float X { get; set; }
        public float Y { get; set; }
    
        public void IncKill() => Kills++;
        public void IncBulletCount() => BulletCount++;
        public void DecLivesRemaining() => LivesRemaining--;
    
        public void SetDead() => IsAlive = false;
    }
}