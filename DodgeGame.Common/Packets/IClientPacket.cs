using DodgeGame.Common.Manager;

namespace DodgeGame.Common.Packets
{
    public interface IClientPacket
    {
        void Process(Client client);
    }
}