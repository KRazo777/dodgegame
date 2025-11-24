using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using DodgeGame.Common.Game;
using System.Linq;
using DodgeGame.Common;

namespace DodgeGame.Server;

/// <summary>
/// Lightweight REST server that runs alongside the game server to expose status endpoints.
/// </summary>
public class RestServer
{
    private const string ApiKeyHeader = "X-Api-Key";
    private readonly string _apiKey = "a012931kjbmc2131STUPIDAPIKEY";
    private readonly HttpListener _listener = new();

    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true // needed because DodgeGame.Common.User exposes data as public fields
    };

    private Task? _listenerTask;
    public readonly ConcurrentDictionary<string, TokenRecord> Tokens = new();

    public RestServer(params string[] prefixes)
    {
        if (prefixes.Length == 0)
            throw new ArgumentException("At least one URI prefix is required for the REST server.", nameof(prefixes));

        foreach (var prefix in prefixes)
        {
            var normalized = prefix.EndsWith('/') ? prefix : prefix + "/";
            _listener.Prefixes.Add(normalized);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_listener.IsListening)
            return _listenerTask ?? Task.CompletedTask;

        _listener.Start();
        Console.WriteLine($"REST server listening on: {string.Join(", ", _listener.Prefixes)}");

        _listenerTask = Task.Run(() => ListenAsync(cancellationToken), cancellationToken);
        return _listenerTask;
    }

    public async Task StopAsync()
    {
        if (!_listener.IsListening)
            return;

        _listener.Stop();
        _listener.Close();

        if (_listenerTask != null)
            await _listenerTask.ConfigureAwait(false);
    }

    private async Task ListenAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            HttpListenerContext? context = null;
            try
            {
                context = await _listener.GetContextAsync().ConfigureAwait(false);
            }
            catch (HttpListenerException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            if (context == null)
                continue;

            _ = Task.Run(() => HandleRequestAsync(context), cancellationToken);
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        try
        {
            AddCorsHeaders(context.Response);

            if (context.Request.HttpMethod.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return;
            }

            var requestPath = context.Request.Url?.AbsolutePath.TrimEnd('/').ToLowerInvariant() ?? string.Empty;
            switch (requestPath)
            {
                case "/health":
                case "":
                    await WriteJsonAsync(context.Response, HttpStatusCode.OK, new { status = "ok" })
                        .ConfigureAwait(false);
                    break;
                case "/api/v1/token":
                    await HandleTokenAsync(context).ConfigureAwait(false);
                    break;
                case "/rooms":
                    await WriteRoomsAsync(context.Response).ConfigureAwait(false);
                    break;
                default:
                    await WriteJsonAsync(context.Response, HttpStatusCode.NotFound, new { error = "Not found" })
                        .ConfigureAwait(false);
                    break;
            }
        }
        catch (Exception ex)
        {
            await WriteJsonAsync(context.Response, HttpStatusCode.InternalServerError, new { error = ex.Message })
                .ConfigureAwait(false);
        }
        finally
        {
            context.Response.OutputStream.Close();
        }
    }

    private async Task WriteRoomsAsync(HttpListenerResponse response)
    {
        var rooms = Server.GameRooms.Values.Select(room => new
        {
            room.RoomId,
            room.RoomName,
            room.HostUniqueId,
            room.IsPrivate,
            playerCount = room.Players.Count,
            entityCount = room.Entities.Count
        });

        await WriteJsonAsync(response, HttpStatusCode.OK, new { rooms }).ConfigureAwait(false);
    }

    private async Task WriteJsonAsync(HttpListenerResponse response, HttpStatusCode statusCode, object payload)
    {
        AddCorsHeaders(response);
        response.StatusCode = (int)statusCode;
        response.ContentType = "application/json";

        await JsonSerializer.SerializeAsync(response.OutputStream, payload, _jsonOptions).ConfigureAwait(false);
    }

    private async Task HandleTokenAsync(HttpListenerContext context)
    {
        if (context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            var tokenQuery = context.Request.QueryString["token"];
            var record = Tokens.Values.FirstOrDefault(tr => tr != null && tr.Token.Equals(tokenQuery), null);
            if (record == null)
            {
                await WriteJsonAsync(context.Response, HttpStatusCode.NotFound, new { error = "Token not found" })
                    .ConfigureAwait(false);
                return;
            }

            var user = await Server.SupabaseClient.AdminAuth.GetUserById(record.UserId);
            if (user == null)
            {
                await WriteJsonAsync(context.Response, HttpStatusCode.NotFound, new { error = "User not found" })
                    .ConfigureAwait(false);
                return;
            }

            var createdUser = new User(user.Id, user.UserMetadata["username"] as string,
                (long)(user.CreatedAt - new DateTime(1970, 1, 1)).TotalMilliseconds);
            
            await WriteJsonAsync(context.Response, HttpStatusCode.OK, new { createdUser }
            ).ConfigureAwait(false);
            return;
        }

        if (context.Request.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
        {
            if (!IsAuthorized(context.Request))
            {
                await WriteJsonAsync(context.Response, HttpStatusCode.Unauthorized, new { error = "Invalid API key" })
                    .ConfigureAwait(false);
                return;
            }
            var payload = await ReadTokenPayloadAsync(context.Request).ConfigureAwait(false);
            Console.WriteLine(payload);
            if (payload == null)
            {
                await WriteJsonAsync(context.Response, HttpStatusCode.BadRequest, new { error = "Invalid payload" })
                    .ConfigureAwait(false);
                return;
            }

            SetOrCreateToken(payload.Token, payload.UserId);
            await WriteJsonAsync(context.Response, HttpStatusCode.Created, new { }).ConfigureAwait(false);
            return;
        }

        if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            if (!IsAuthorized(context.Request))
            {
                await WriteJsonAsync(context.Response, HttpStatusCode.Unauthorized, new { error = "Invalid API key" })
                    .ConfigureAwait(false);
                return;
            }
            var payload = await ReadTokenPayloadAsync(context.Request).ConfigureAwait(false);
            Console.WriteLine(payload);
            if (payload == null)
            {
                await WriteJsonAsync(context.Response, HttpStatusCode.BadRequest, new { error = "Invalid payload" })
                    .ConfigureAwait(false);
                return;
            }

            if (!Tokens.ContainsKey(payload.UserId))
            {
                await WriteJsonAsync(context.Response, HttpStatusCode.NotFound, new { error = "User token not found" })
                    .ConfigureAwait(false);
                return;
            }

            SetOrCreateToken(payload.Token, payload.UserId);
            await WriteJsonAsync(context.Response, HttpStatusCode.OK, new { }).ConfigureAwait(false);
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
        context.Response.Headers.Add("Allow", "PUT, POST");
    }

    private void SetOrCreateToken(string token, string userId)
    {
        var tokenRecord = new TokenRecord(token, userId);
        Tokens[userId] = tokenRecord;
    }

    private bool IsAuthorized(HttpListenerRequest request)
    {
        var providedKey = request.Headers[ApiKeyHeader];
        return !string.IsNullOrWhiteSpace(providedKey) && providedKey == _apiKey;
    }

    private static void AddCorsHeaders(HttpListenerResponse response)
    {
        response.Headers["Access-Control-Allow-Origin"] = "*";
        response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, OPTIONS";
        response.Headers["Access-Control-Allow-Headers"] = "Content-Type, X-Api-Key";
    }

    private async Task<TokenPayload?> ReadTokenPayloadAsync(HttpListenerRequest request)
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<TokenPayload>(request.InputStream, _jsonOptions)
                .ConfigureAwait(false);
        }
        catch
        {
            return null;
        }
    }

    private record TokenPayload(
        [property: JsonPropertyName("token")] string Token,
        [property: JsonPropertyName("user_id")]
        string UserId);

    public record TokenRecord(string Token, string UserId);
}
