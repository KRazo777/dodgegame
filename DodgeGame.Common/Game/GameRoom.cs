using System.Collections.Generic;

namespace DodgeGame.Common.Game
{
    public class GameRoom
    {
        public Dictionary<string, Entity> Entities { get; } = new Dictionary<string, Entity>();
        public Dictionary<string, Player> Players { get; } = new Dictionary<string, Player>();
    
        public string HostUniqueId { get; }
        public string RoomId { get; }
        public string RoomName { get; set; }
        public string RoomPassword { get; set; }
        public bool IsPrivate { get; set; }

        public GameRoom(string hostUniqueId, string roomId, string roomName)
        {
            HostUniqueId = hostUniqueId;
            RoomId = roomId;
            RoomName = roomName;
        }
    }
}