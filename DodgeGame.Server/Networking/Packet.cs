using Riptide;
using Client = DodgeGame.Server.Manager.Client;

namespace DodgeGame.Server.Networking;

public abstract class Packet
{
    public abstract void Deserialize(Message message);
    public abstract Message Serialize();

    public abstract void Process(Client client);
}