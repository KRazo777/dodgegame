using Riptide;
using DodgeGame.Common;

namespace DodgeGame.Common.Manager
{
    public class Client
    {
        private ushort _identifier;
        public Connection Connection { get; }
        public User User { get; set; }

        public Client(Connection connection)
        {
            _identifier = connection.Id;
            Connection = connection;
        }
    
        public ushort Identifier => _identifier;

    }
}
