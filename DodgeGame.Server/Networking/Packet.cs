using Riptide;
using Client = DodgeGameBackend.DodgeGame.Server.Manager.Client;

namespace DodgeGameBackend.DodgeGame.Server.Networking;

public abstract class Packet
{
    public abstract void Deserialize(Message message);
    public abstract Message Serialize();

    public abstract void Process(Client client);
}