using System;
using System.Collections.Generic;
using DodgeGame.Common.Manager;
using Riptide;

namespace DodgeGame.Common.Game
{
    public class GameRoom : ISerializable<GameRoom>
    {
        public Dictionary<string, Entity> Entities { get; } = new Dictionary<string, Entity>();
        public Dictionary<string, Player> Players { get; } = new Dictionary<string, Player>();
    
        public SpawnManager SpawnManager { get; } = new SpawnManager();
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
            RoomPassword = string.Empty;
        }

        public void Serialize(Message message)
        {
            message.AddString(HostUniqueId);
            message.AddString(RoomId);
            message.AddString(OwnerName);
            // Riptide's AddString expects non-null; use empty string if password is unset.
            message.AddString(RoomPassword ?? string.Empty);
            message.AddBool(IsPrivate);
            message.AddBool(IsStarted);
            
            message.AddUInt((uint)Players.Count);
            foreach (var player in Players.Values)
            {
                player.Serialize(message);
            }
        }

        public static GameRoom Deserialize(Message message)
        {
            var room = new GameRoom(
                message.GetString(),
                message.GetString(),
                message.GetString())
            {
                RoomPassword = message.GetString(),
                IsPrivate = message.GetBool(),
                IsStarted = message.GetBool()
            };

            var count = message.GetUInt();
            for (var i = 0; i < count; i++)
            {
                var player = Player.Deserialize(message);
                room.Players.Add(player.Id, player);
            }

            return room;
        }

        
    }
}
