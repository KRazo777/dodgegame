using System.Collections.Concurrent;
using DodgeGame.Common.Game;
using DodgeGame.Common.Manager;

namespace DodgeGame.Common
{
    public interface IGameServer
    {
        Riptide.Server Server { get; }
        ConcurrentDictionary<string, GameRoom> GameRooms { get; }
        
        void Disconnect(Client client);
    }
}