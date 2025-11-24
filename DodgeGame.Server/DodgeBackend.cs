using DodgeGame.Server.Authentication;

namespace DodgeGame.Server;

public class DodgeBackend
{
    public static readonly SupabaseClient SupabaseClient  = new();
    public static GameServer GameServer = new(); 
    public static RestServer RestServer { get; } = new("http://localhost:5000/");
    
    
    public static async Task Main(string[] args)
    {
        await SupabaseClient.Initialize();
        
        
        using var cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };
        
        var restTask = RestServer.StartAsync(cancellationTokenSource.Token);
        GameServer.Start();
        
        while (!cancellationTokenSource.IsCancellationRequested)
        {
            GameServer.Server.Update();
            GameServer.ConnectionHandler.Update();
            Thread.Sleep(1);
        }
        
        await RestServer.StopAsync();
        GameServer.Server.Stop();
        await restTask;
        
    }
}