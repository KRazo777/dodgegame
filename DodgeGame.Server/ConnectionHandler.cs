using DodgeGame.Common.Game;
using DodgeGame.Server.Networking;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;
using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Clientbound;
using DodgeGame.Common.Packets.Serverbound;
using GameListPacket = DodgeGame.Common.Packets.Clientbound.GameListPacket;
using HandshakePacket = DodgeGame.Common.Packets.Serverbound.HandshakePacket;

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
        Console.WriteLine("Client connected: " + args.Client.Id);
        args.Client.CanQualityDisconnect = false;
        Connections[args.Client.Id] = new Client(args.Client);
    }

    public void OnClientDisconnect(object? sender, ServerDisconnectedEventArgs args)
    {
        Connections.Remove(args.Client.Id);
        _lastClientPingAt.Remove(args.Client.Id);
    }

    public void OnMessageReceived(object? sender, MessageReceivedEventArgs args)
    {
        var connection = args.FromConnection;
        var messageId = args.MessageId;
        var message = args.Message;
        if (!Connections.TryGetValue(connection.Id, out var client))
        {
            return;
        }

        client = Connections[connection.Id];

        var packet = _packetHandler.CreateServerboundInstance(messageId);
        if (packet == null)
        {
            // throw some message / error
            return;
        }

        Console.WriteLine("Received packet " + messageId + " from " + connection.Id);

        packet.Deserialize(message);
        packet.Process(client);

        if (messageId == PacketIds.Serverbound.Ping)
        {
            var ping = (PingPacket)packet;
            _lastClientPingAt[client.Identifier] = DateTime.UtcNow;

            client.Connection.Send(new PongPacket(ping.SentAtTicks).Serialize());
            return;
        }

        if (messageId == PacketIds.Serverbound.Movement)
        {
            var movement = (DodgeGame.Common.Packets.Serverbound.MovementPacket)packet;
            Console.WriteLine("Player " + movement.UniqueId + " moved to " + movement.X + ", " + movement.Y);

            var room = Server.GameRooms.Values.FirstOrDefault(
                gameRoom => gameRoom != null && gameRoom.Players.ContainsKey(movement.UniqueId), null);
            if (room == null) return;
            room.Players[movement.UniqueId].X = movement.X;
            room.Players[movement.UniqueId].Y = movement.Y;


            foreach (var playerClient in room.Players.Values.Select(player =>
                         Connections.Values.First(c => c.User.UniqueId.Equals(player.Id))))
            {
                _outgoingPacketManager.SendPacket(playerClient, movement);
            }

            return;
        }

        if (messageId == PacketIds.Serverbound.GameList)
        {
            client.Connection.Send(new GameListPacket(Server.GameRooms.Values.ToArray()).Serialize());
            return;
        }

        if (messageId == PacketIds.Serverbound.GameJoin)
        {
            var joinGamePacket = (JoinGamePacket)packet;
            if (!Server.GameRooms.TryGetValue(joinGamePacket.RoomId, out var room)) return;
            
            var player = new Player(client.User.UniqueId, client.User.Username, EntityType.Player);
            room.Players.Add(player.Id, player);
            
            //TODO: SEND GAME JOIN TO PLAYER, SEND OTHER PLAYER INFO TO PLAYER
            return;
        }

        if (messageId == PacketIds.Serverbound.Handshake)
        {
            var handshake = (HandshakePacket)packet;
            Console.WriteLine("Received handshake from " + handshake.Username);
            return;
        }

        // Send an acknowledgement back to the client for the received packet.
        // _outgoingPacketManager.SendAcknowledgementFor(client, packet);
    }

    public void Update()
    {
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
                    Console.WriteLine("Disconnected inactive client " + client.Identifier);
                }
            }
        }
    }
}