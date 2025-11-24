using DodgeGame.Common.Manager;

namespace DodgeGame.Common.Packets
{
    public interface IServerPacket
    {
        void Process(IGameServer gameServer, Client client);
    }
}