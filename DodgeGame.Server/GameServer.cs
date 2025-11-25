using System.Collections.Concurrent;
using DodgeGame.Common;
using DodgeGame.Common.Game;
using DodgeGame.Common.Manager;
using DodgeGame.Server.Authentication;
using Riptide.Utils;

namespace DodgeGame.Server;

public class GameServer : IGameServer
{
    private Riptide.Server _server = new();
    private ConcurrentDictionary<string, GameRoom> _gameRooms = new();
    
    public readonly ConnectionHandler ConnectionHandler = new();
    public Riptide.Server Server => _server;
    public ConcurrentDictionary<string, GameRoom> GameRooms => _gameRooms;
    public void Start()
    {
        RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);

        Server.Start(2442, ushort.MaxValue - 1, 0, false);

        Server.ClientConnected += ConnectionHandler.OnClientConnect;
        Server.ClientDisconnected += ConnectionHandler.OnClientDisconnect;
        Server.MessageReceived += ConnectionHandler.OnMessageReceived;
    }

    public void Disconnect(Client client)
    {
        Server.DisconnectClient(client.Connection);
    }

    public Client? GetClient(string uniqueId)
    {
        return ConnectionHandler.Connections.Values.FirstOrDefault(x => x.User != null && x.User.UniqueId == uniqueId);
    }
}
