using DodgeGameBackend.DodgeGame.Server.Game;

namespace DodgeGameBackend.DodgeGame.Server.Manager;

public class User(string uniqueId, string username, long dateCreated)
{
    public readonly string UniqueId = uniqueId;
    public readonly string Username = username;
    public readonly long DateCreated = dateCreated;
    public Player Player { get; set; }
}