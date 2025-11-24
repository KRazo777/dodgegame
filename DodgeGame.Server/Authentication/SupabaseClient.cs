using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Client = Supabase.Client;
using User = Supabase.Gotrue.User;

namespace DodgeGame.Server.Authentication;

public class SupabaseClient
{
    private static readonly string SUPABASE_URL = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? "dev_url.com";
    private static readonly string SUPABASE_KEY = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? "dev-key";

    public IGotrueClient<User, Session> Auth => _supabaseClient.Auth;
    public IGotrueAdminClient<User> AdminAuth => _supabaseClient.AdminAuth(SUPABASE_KEY);
    
    private SupabaseOptions _options = new SupabaseOptions
    {
        AutoConnectRealtime = true
    };

    private Client _supabaseClient;

    public SupabaseClient()
    {
        _supabaseClient = new Client(SUPABASE_URL, SUPABASE_KEY, _options);
    }

    public async Task Initialize()
    {
        await _supabaseClient.InitializeAsync();
    }
}