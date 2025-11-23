using System;
using Riptide;

namespace DodgeGame.Common.Packets
{
    public class PendingPacket
    {
        public PendingPacket(Message message, Connection connection)
        {
            Message = message;
            Connection = connection;
            Touch();
        }

        public Message Message { get; }
        public Connection Connection { get; }
        public DateTime LastSentUtc { get; private set; }

        public void Touch()
        {
            LastSentUtc = DateTime.UtcNow;
        }
    }
}