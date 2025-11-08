using System.Numerics;

public abstract class Entity
{
    private string UniqueId { get; }
    private EntityType EntityType { get; }
    private Vector2 _position { get; set; }
    private Vector2 _velocity { get; set; }

    protected Entity(string uniqueId, EntityType entityType)
    {
        UniqueId = uniqueId;
        EntityType = entityType;
    }
}