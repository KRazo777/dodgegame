using System.Collections.Generic;

namespace DodgeGame.Common.Game
{
    public class GameRoom
    {
        public Dictionary<string, Entity> Entities { get; } = new Dictionary<string, Entity>();
        public Dictionary<string, Player> Players { get; } = new Dictionary<string, Player>();
    
        public string HostUniqueId { get; }
        public string RoomId { get; }
        public string OwnerName { get; set; }
        public string RoomPassword { get; set; }
        public bool IsPrivate { get; set; }
        
        public bool IsStarted { get; set; }

        public GameRoom(string hostUniqueId, string roomId, string ownerName)
        {
            HostUniqueId = hostUniqueId;
            RoomId = roomId;
            OwnerName = ownerName;
        }
    }
}