using DodgeGameBackend.DodgeGame.Server.Game;
using Riptide.Utils;

namespace DodgeGameBackend.DodgeGame.Server;

public class Server
{
    public static ConnectionHandler ConnectionHandler { get; } = new();
    public static Dictionary<string, GameRoom> GameRooms { get; } = new();
    public static void Main(string[] args)
    {
        RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);
        var server = new Riptide.Server();
        
        server.Start(2442, ushort.MaxValue);

        while (true)
        {
            server.Update();
            server.ClientConnected += ConnectionHandler.OnClientConnect;
            server.ClientDisconnected += ConnectionHandler.OnClientDisconnect;
            server.MessageReceived += ConnectionHandler.OnMessageReceived;
        }
    }
}