using DodgeGame.Common.Game;
using DodgeGame.Common.Manager;
using Riptide.Utils;

namespace DodgeGame.Server;

public class Server
{
    public static Riptide.Server GameServer;
    public static ConnectionHandler ConnectionHandler { get; } = new();
    public static Dictionary<string, GameRoom> GameRooms { get; } = new();
    public static void Main(string[] args)
    {
        RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);
        GameServer = new Riptide.Server();
        
        GameServer.Start(2442, ushort.MaxValue);

        GameServer.ClientConnected += ConnectionHandler.OnClientConnect;
        GameServer.ClientDisconnected += ConnectionHandler.OnClientDisconnect;
        GameServer.MessageReceived += ConnectionHandler.OnMessageReceived;

        while (true)
        {
            GameServer.Update();
            ConnectionHandler.Update();
        }
    }

    public static void Disconnect(Client client)
    {
        GameServer.DisconnectClient(client.Connection);
    }
}
