using System.Numerics;
using Riptide;

namespace DodgeGame.Common.Game
{
    public abstract class Entity
    {
        private string UniqueId { get; }
        private EntityType EntityType { get; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        protected Entity(string uniqueId, EntityType entityType)
        {
            UniqueId = uniqueId;
            EntityType = entityType;
        }

        public string Id => UniqueId;
    }
}
