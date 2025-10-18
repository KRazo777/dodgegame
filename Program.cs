using DodgeGameBackend.Server;
using Riptide;
using Riptide.Utils;

public class Program
{
    
    public static void Main(string[] args)
    {
        RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);
        var server = new Server();
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