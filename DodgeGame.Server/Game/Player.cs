namespace DodgeGameBackend.DodgeGame.Server.Game;

public class Player(string uniqueId, string name, EntityType entityType) : Entity(uniqueId, entityType)
{
    public readonly string Name = name;
    public byte Kills { get; set; }
    public ushort BulletCount { get; set; }
    public bool IsAlive { get; set; }
    public byte LivesRemaining { get; set; }
    
    public void IncKill() => Kills++;
    public void IncBulletCount() => BulletCount++;
    public void DecLivesRemaining() => LivesRemaining--;
    
    public void SetDead() => IsAlive = false;
}