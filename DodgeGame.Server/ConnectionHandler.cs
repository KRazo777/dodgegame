using DodgeGameBackend.Server.Packet;
using Riptide;

namespace DodgeGameBackend.DodgeGame.Server;

public class ConnectionHandler
{
    public readonly Dictionary<ushort, Client> Connections = new();

    private readonly PacketHandler _packetHandler = new();

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

        if (!_packetHandler.RegisteredPackets.TryGetValue(messageId, out var packet))
        {
            // throw some message / error
            return;
        }

        packet.Deserialize(message);
        packet.Process(client);
    }
}