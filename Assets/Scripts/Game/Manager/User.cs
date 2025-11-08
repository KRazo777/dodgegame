public class User
{
    public readonly string UniqueId;
    public readonly string Username;
    public readonly long DateCreated;

    public User(string uniqueId, string username, long dateCreated)
    {
        UniqueId = uniqueId;
        Username = username;
        DateCreated = dateCreated;
    }

    public Player Player { get; set; }
}