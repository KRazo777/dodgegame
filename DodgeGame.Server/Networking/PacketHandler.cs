namespace DodgeGame.Server.Networking;

public class PacketHandler
{
    public readonly Dictionary<ushort, Packet> RegisteredPackets = new();
}