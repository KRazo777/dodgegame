using Riptide;
using Client = DodgeGameBackend.DodgeGame.Server.Client;

namespace DodgeGameBackend.Server.Packet;

public abstract class Packet
{
    public abstract void Deserialize(Message message);
    public abstract Message Serialize();

    public abstract void Process(Client client);
}