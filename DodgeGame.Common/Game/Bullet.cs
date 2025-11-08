namespace DodgeGame.Common.Game;

public class Bullet(string uniqueId, string ownerUniqueId, EntityType entityType) : Entity(uniqueId, entityType)
{
    public readonly string OwnerUniqueId = ownerUniqueId;
}