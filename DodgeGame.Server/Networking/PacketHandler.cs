using DodgeGame.Common.Packets;
using CB = DodgeGame.Common.Packets.Clientbound;
using SB = DodgeGame.Common.Packets.Serverbound;

namespace DodgeGame.Server.Networking;

public class PacketHandler
{
    public readonly Dictionary<ushort, Type> ServerboundPackets = new();
    public readonly Dictionary<ushort, Type> ClientboundPackets = new();

    public PacketHandler()
    {
        RegisterServerbound<SB.HandshakePacket>();
        RegisterServerbound<SB.AcknowledgementPacket>();
        RegisterServerbound<SB.PingPacket>();
        RegisterServerbound<SB.JoinGameRequestPacket>();
        RegisterServerbound<SB.MovementPacket>();

        RegisterClientbound<CB.HandshakePacket>();
        RegisterClientbound<CB.AcknowledgementPacket>();
        RegisterClientbound<CB.PongPacket>();
        RegisterClientbound<CB.JoinGameConfirmedPacket>();
        RegisterClientbound<CB.PlayerDetailsPacket>();
        RegisterClientbound<CB.SpawnPlayerPacket>();
        RegisterClientbound<CB.MovementPacket>();
    }

    public Packet? CreateServerboundInstance(ushort messageId)
    {
        if (!ServerboundPackets.TryGetValue(messageId, out var type))
            return null;

        return Activator.CreateInstance(type) as Packet;
    }

    public Packet? CreateClientboundInstance(ushort messageId)
    {
        if (!ClientboundPackets.TryGetValue(messageId, out var type))
            return null;

        return Activator.CreateInstance(type) as Packet;
    }

    public void RegisterServerbound<TPacket>() where TPacket : Packet, new()
    {
        var instance = new TPacket();
        ServerboundPackets[instance.Id] = typeof(TPacket);
    }

    public void RegisterClientbound<TPacket>() where TPacket : Packet, new()
    {
        var instance = new TPacket();
        ClientboundPackets[instance.Id] = typeof(TPacket);
    }
}
