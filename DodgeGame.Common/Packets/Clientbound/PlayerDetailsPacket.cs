using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PlayerDetailsPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.PlayerDetails;

        public string UniqueId { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public byte Kills { get; private set; }
        public ushort BulletCount { get; private set; }
        public bool IsAlive { get; private set; }
        public byte LivesRemaining { get; private set; }

        public PlayerDetailsPacket()
        {
        }

        public PlayerDetailsPacket(string uniqueId, string name, byte kills, ushort bulletCount, bool isAlive, byte livesRemaining)
        {
            UniqueId = uniqueId;
            Name = name;
            Kills = kills;
            BulletCount = bulletCount;
            IsAlive = isAlive;
            LivesRemaining = livesRemaining;
        }

        public override void Deserialize(Message message)
        {
            UniqueId = message.GetString();
            Name = message.GetString();
            Kills = message.GetByte();
            BulletCount = message.GetUShort();
            IsAlive = message.GetBool();
            LivesRemaining = message.GetByte();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(UniqueId);
            message.AddString(Name);
            message.AddByte(Kills);
            message.AddUShort(BulletCount);
            message.AddBool(IsAlive);
            message.AddByte(LivesRemaining);
            return message;
        }

        public void Process(Client client)
        {
            // Clientbound packet: handled on client application to set up the local player.
        }
    }
}
