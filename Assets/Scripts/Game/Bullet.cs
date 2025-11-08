public class Bullet : Entity
{
    public readonly string OwnerUniqueId;

    public Bullet(string uniqueId, string ownerUniqueId, EntityType entityType) : base(uniqueId, entityType)
    {
        OwnerUniqueId = ownerUniqueId;
    }
}