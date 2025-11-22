using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Clientbound;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Server.Networking;

public class OutgoingPacketManager
{
    private class PendingPacket
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

    private readonly Dictionary<ushort, Dictionary<ushort, PendingPacket>> _pendingByClient = new();
    private readonly Dictionary<ushort, ushort> _nextSequenceIdByClient = new();
    private readonly TimeSpan _resendInterval = TimeSpan.FromMilliseconds(500);

    public ushort SendPacket(Client client, Packet packet)
    {
        var sequenceId = NextSequenceIdFor(client.Identifier);
        packet.SequenceId = sequenceId;

        var message = packet.Serialize();
        client.Connection.Send(message);

        TrackPending(client.Identifier, sequenceId, message, client.Connection);
        return sequenceId;
    }

    public void HandleAcknowledgement(ushort clientId, ushort acknowledgedSequenceId)
    {
        if (_pendingByClient.TryGetValue(clientId, out var pending))
        {
            pending.Remove(acknowledgedSequenceId);
        }
    }

    public void TickResends()
    {
        var now = DateTime.UtcNow;

        foreach (var (_, pendingPackets) in _pendingByClient.ToList())
        {
            foreach (var (_, pending) in pendingPackets.ToList())
            {
                if (now - pending.LastSentUtc >= _resendInterval)
                {
                    pending.Touch();
                    pending.Connection.Send(pending.Message);
                }
            }
        }
    }

    public void SendAcknowledgementFor(Client client, Packet receivedPacket)
    {
        var ack = new AcknowledgementPacket(receivedPacket.Id)
        {
            AcknowledgedSequenceId = receivedPacket.SequenceId
        };

        SendPacket(client, ack);
    }

    private ushort NextSequenceIdFor(ushort clientId)
    {
        if (!_nextSequenceIdByClient.TryGetValue(clientId, out var next))
        {
            next = 1;
        }

        _nextSequenceIdByClient[clientId] = (ushort)(next + 1);
        return next;
    }

    private void TrackPending(ushort clientId, ushort sequenceId, Message message, Connection connection)
    {
        if (!_pendingByClient.TryGetValue(clientId, out var pending))
        {
            pending = new Dictionary<ushort, PendingPacket>();
            _pendingByClient[clientId] = pending;
        }

        pending[sequenceId] = new PendingPacket(message, connection);
    }
}
