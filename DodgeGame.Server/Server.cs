using System.Collections.Concurrent;
using DodgeGame.Common.Game;
using DodgeGame.Common.Manager;
using DodgeGame.Server.Authentication;
using Riptide.Utils;

namespace DodgeGame.Server;

public class Server
{
    public static Riptide.Server GameServer;
    public static ConnectionHandler ConnectionHandler { get; } = new();
    public static ConcurrentDictionary<string, GameRoom> GameRooms { get; } = new();
    public static SupabaseClient SupabaseClient { get; } = new();
    
    public static RestServer RestServer { get; } = new RestServer("http://localhost:5000/");

    public static async Task Main(string[] args)
    {
        await SupabaseClient.Initialize();
        
        RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);
        GameServer = new Riptide.Server();

        GameServer.Start(2442, ushort.MaxValue - 1, 0, false);

        GameServer.ClientConnected += ConnectionHandler.OnClientConnect;
        GameServer.ClientDisconnected += ConnectionHandler.OnClientDisconnect;
        GameServer.MessageReceived += ConnectionHandler.OnMessageReceived;

        var devRoom = new GameRoom("DEV-UUID", "devroom", "DEV ROOM");
        GameRooms.TryAdd(devRoom.RoomId, devRoom);

        using var cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };
        
        var restTask = RestServer.StartAsync(cancellationTokenSource.Token);

        while (!cancellationTokenSource.IsCancellationRequested)
        {
            GameServer.Update();
            ConnectionHandler.Update();
            Thread.Sleep(1);
        }

        await RestServer.StopAsync();
        GameServer.Stop();
        await restTask;
    }

    public static void Disconnect(Client client)
    {
        GameServer.DisconnectClient(client.Connection);
    }
}
