namespace DodgeGameBackend.Server.Packet;

public class PacketHandler
{
    public readonly Dictionary<ushort, Packet> RegisteredPackets = new();
}