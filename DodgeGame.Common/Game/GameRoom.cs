namespace DodgeGame.Common.Game;

public class GameRoom
{
    public Dictionary<string, Entity> Entities { get; } = new();
    public Dictionary<string, Player> Players { get; } = new();
    
    public string HostUniqueId { get; }
    public string RoomId { get; }
    public string RoomName { get; set; }
    public string RoomPassword { get; set; }
    public bool IsPrivate { get; set; }
}