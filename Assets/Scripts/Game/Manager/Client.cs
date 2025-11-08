using Riptide;

public class Client
{
    private ushort _identifier;

    public Client(Connection connection)
    {
        _identifier = connection.Id;
        Connection = connection;
    }

    public Connection Connection { get; }
    public User User { get; set; }
    
    public ushort Identifier => _identifier;
}