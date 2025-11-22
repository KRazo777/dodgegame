using DodgeGame.Server.Networking;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;
using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Clientbound;

namespace DodgeGame.Server;

public class ConnectionHandler
{
    public readonly Dictionary<ushort, Client> Connections = new();

    private readonly PacketHandler _packetHandler = new();
    private readonly OutgoingPacketManager _outgoingPacketManager = new();
    private readonly Dictionary<ushort, DateTime> _lastClientPingAt = new();
    private readonly TimeSpan _clientPingTimeout = TimeSpan.FromSeconds(5);

    public void OnClientConnect(object? sender, ServerConnectedEventArgs args)
    {
        Connections[args.Client.Id] = new Client(args.Client);
    }

    public void OnClientDisconnect(object? sender, ServerDisconnectedEventArgs args)
    {
        Connections.Remove(args.Client.Id);
    }

    public void OnMessageReceived(object? sender, MessageReceivedEventArgs args)
    {
        var connection = args.FromConnection;
        var messageId = args.MessageId;
        var message = args.Message;
        var client = this.Connections[connection.Id];

        var packet = _packetHandler.CreateServerboundInstance(messageId);
        if (packet == null)
        {
            // throw some message / error
            return;
        }

        packet.Deserialize(message);
        packet.Process(client);

        // Handle acknowledgements for serverbound acknowledgements and pings.
        // if (messageId == PacketIds.Serverbound.Acknowledgement)
        // {
        //     var ack = (AcknowledgementPacket)packet;
        //     _outgoingPacketManager.HandleAcknowledgement(client.Identifier, ack.AcknowledgedSequenceId);
        //     return;
        // }

        if (messageId == PacketIds.Serverbound.Ping)
        {
            var pong = (PongPacket)packet;
            _outgoingPacketManager.HandleAcknowledgement(client.Identifier, pong.PingSequenceId);
            _lastClientPingAt[client.Identifier] = DateTime.UtcNow;
        }

        if (messageId == PacketIds.Serverbound.Movement)
        {
            var movement = (MovementPacket)packet;
            var room = Server.GameRooms.Values.First(gameRoom => gameRoom.Players.ContainsKey(movement.UniqueId));
            room.Players[movement.UniqueId].X = movement.X;
            room.Players[movement.UniqueId].Y = movement.Y;
            
            foreach (var playerClient in room.Players.Values.Select(player => Connections.Values.First(_client => _client.User.UniqueId.Equals(player.Id))))
            {
                _outgoingPacketManager.SendPacket(playerClient, movement);
            }
        }

        // Send an acknowledgement back to the client for the received packet.
        _outgoingPacketManager.SendAcknowledgementFor(client, packet);
    }

    public void Update()
    {
        _outgoingPacketManager.TickResends();
        DisconnectInactiveClients();
    }

    private void DisconnectInactiveClients()
    {
        var now = DateTime.UtcNow;
        foreach (var client in Connections.Values.ToList())
        {
            if (_lastClientPingAt.TryGetValue(client.Identifier, out var lastClientPing))
            {
                if (now - lastClientPing > _clientPingTimeout)
                {
                    Server.Disconnect(client);
                    Connections.Remove(client.Identifier);
                }
            }
        }
    }
}