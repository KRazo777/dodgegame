using Riptide;
using DodgeGame.Common;
using DodgeGame.Common.Packets;

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

        public void SendPacket(Packet packet)
        {
            Connection.Send(packet.Serialize());
        }
    
        public ushort Identifier => _identifier;

    }
}
