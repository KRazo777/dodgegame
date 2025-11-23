using DodgeGame.Common.Packets;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Server.Networking;

public class OutgoingPacketManager
{
    public ushort SendPacket(Client client, Packet packet)
    {
        var message = packet.Serialize(); // Packet implementations already use MessageSendMode.Reliable.
        client.Connection.Send(message);
        return 0;
    }
}
