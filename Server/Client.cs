using Riptide;

namespace DodgeGameBackend.Server;

public class Client
{
    private Connection Connection { get; }

    public Client(Connection connection)
    {
        this.Connection = connection;
    }
}