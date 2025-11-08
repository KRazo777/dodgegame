using DodgeGame.Common.Game;

namespace DodgeGame.Common;

public class User(string uniqueId, string username, long dateCreated)
{
    public readonly string UniqueId = uniqueId;
    public readonly string Username = username;
    public readonly long DateCreated = dateCreated;
    public Player Player { get; set; }
}