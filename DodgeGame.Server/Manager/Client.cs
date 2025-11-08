using Riptide;
using DodgeGame.Common;

namespace DodgeGame.Server.Manager;

public class Client(Connection connection)
{
    private ushort _identifier = connection.Id;
    public Connection Connection { get; } = connection;
    public User User { get; set; }
    
    public ushort Identifier => _identifier;
}