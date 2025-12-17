using DodgeGame.Common;
using DodgeGame.Common.Game;
using DodgeGame.Server.Networking;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;
using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Clientbound;
using DodgeGame.Common.Packets.Serverbound;
using BulletFiredPacket = DodgeGame.Common.Packets.Serverbound.BulletFiredPacket;
using HandshakePacket = DodgeGame.Common.Packets.Serverbound.HandshakePacket;
using MovementPacket = DodgeGame.Common.Packets.Serverbound.MovementPacket;
using RequestGamePacket = DodgeGame.Common.Packets.Serverbound.RequestGameListPacket;
using LeaveGamePacket = DodgeGame.Common.Packets.Serverbound.LeaveGamePacket;

namespace DodgeGame.Server;

public class ConnectionHandler
{
    public readonly Dictionary<ushort, Client> Connections = new();

    private readonly PacketHandler _packetHandler = new();
    private readonly OutgoingPacketManager _outgoingPacketManager = new();
    private readonly Dictionary<ushort, DateTime> _lastClientPingAt = new();
    private readonly TimeSpan _clientPingTimeout = TimeSpan.FromSeconds(5);

    public ConnectionHandler()
    {
        _packetHandler.RegisterServerbound<HandshakePacket>();
        _packetHandler.RegisterServerbound<PingPacket>();
        _packetHandler.RegisterServerbound<MovementPacket>();
        _packetHandler.RegisterServerbound<RequestGameListPacket>();
        _packetHandler.RegisterServerbound<ClientAuthenticationPacket>();
        _packetHandler.RegisterServerbound<CreateRoomPacket>();
        _packetHandler.RegisterServerbound<JoinGamePacket>();
        _packetHandler.RegisterServerbound<BulletHitPacket>();
        _packetHandler.RegisterServerbound<BulletFiredPacket>();
        _packetHandler.RegisterServerbound<LeaveGamePacket>();
    }

    public void OnClientConnect(object? sender, ServerConnectedEventArgs args)
    {
        Console.WriteLine("Client connected: " + args.Client.Id);
        args.Client.CanQualityDisconnect = false;
        Connections[args.Client.Id] = new Client(args.Client);
    }

    public void OnClientDisconnect(object? sender, ServerDisconnectedEventArgs args)
    {
        if (Connections.TryGetValue(args.Client.Id, out var client))
        {
            if (client.User != null && client.User.Player != null)
            {
                var room = client.User.Player.GameRoom;
                if (room != null)
                {
                    if (room.Players.ContainsKey(client.User.UniqueId))
                    {
                        room.Players.Remove(client.User.UniqueId);
                        Console.WriteLine($"[GAME] Removed {client.User.Username} from Room {room.RoomId}");
                    }

                    if (room.Players.Count == 0)
                    {
                        
                        if (DodgeBackend.GameServer.GameRooms.TryRemove(room.RoomId, out _))
                        {
                            Console.WriteLine($"[GAME] Room {room.RoomId} is empty. Destroyed.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[GAME] Room {room.RoomId} still has {room.Players.Count} players.");
                    }
                }
            }
        }

        Connections.Remove(args.Client.Id);
        _lastClientPingAt.Remove(args.Client.Id);
        Console.WriteLine($"[NET] Client {args.Client.Id} disconnected.");
    }

    public void OnMessageReceived(object? sender, MessageReceivedEventArgs args)
    {
        var connection = args.FromConnection;
        var messageId = (PacketIds.Serverbound)args.MessageId;
        var message = args.Message;
        if (!Connections.TryGetValue(connection.Id, out var client))
        {
            return;
        }

        client = Connections[connection.Id];

        var packet = _packetHandler.CreateServerboundInstance((ushort)messageId);
        if (packet == null)
        {
            // throw some message / error
            return;
        }

        Console.WriteLine("Received packet " + messageId + " from " + connection.Id);

        packet.Deserialize(message);
        ((IServerPacket)packet).Process(DodgeBackend.GameServer, client);

        if (messageId == PacketIds.Serverbound.Ping)
        {
            _lastClientPingAt[client.Identifier] = DateTime.UtcNow;
            return;
        }

        if (messageId == PacketIds.Serverbound.Handshake)
        {
            var handshake = (HandshakePacket)packet;
            Console.WriteLine("Received handshake from " + handshake.Username);
            return;
        }

        if (messageId == PacketIds.Serverbound.ClientAuth)
        {
            var auth = (ClientAuthenticationPacket)packet;
            var token = auth.Token.Substring(0, auth.Token.Length - 1);
            if (!DodgeBackend.RestServer.Tokens.IsEmpty)
            {
                Console.WriteLine(DodgeBackend.RestServer.Tokens.First());
            }

            User createdUser;
            var record =
                DodgeBackend.RestServer.Tokens.Values.FirstOrDefault(tr => tr != null && tr.Token.Equals(token));
            if (!token.StartsWith("dev"))
            {
                if (record == null)
                {
                    Console.WriteLine("Token not found");
                    return;
                }

                var user = DodgeBackend.SupabaseClient.AdminAuth.GetUserById(record.UserId).GetAwaiter().GetResult();
                Console.WriteLine("User " + user?.UserMetadata["username"] + " logged in");
                if (user == null)
                {
                    Console.WriteLine("User not found");
                    return;
                }

                createdUser = new User(user.Id, user.UserMetadata["username"] as string,
                    (long)(user.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds);
            }
            else
            {
                createdUser = new User(record.UserId, token, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
            
            client.User = createdUser;

            client.Connection.Send(new ClientAuthenticatedPacket(createdUser).Serialize());
            
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
                    DodgeBackend.GameServer.Disconnect(client);
                    Connections.Remove(client.Identifier);
                    Console.WriteLine("Disconnected inactive client " + client.Identifier);
                }
            }
        }
    }
}