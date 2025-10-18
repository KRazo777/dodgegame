using DodgeGameBackend.Server;
using Riptide.Utils;

namespace DodgeGameBackend.DodgeGame.Server;

public class Server
{
    
    public static void Main(string[] args)
    {
        RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);
        var server = new Riptide.Server();
        var connectionHandler = new ConnectionHandler();
        
        server.Start(2442, UInt16.MaxValue);

        while (true)
        {
            server.Update();
            server.ClientConnected += connectionHandler.OnClientConnect;
            server.ClientDisconnected += connectionHandler.OnClientDisconnect;
            server.MessageReceived += connectionHandler.OnMessageReceived;
        }
    }
}