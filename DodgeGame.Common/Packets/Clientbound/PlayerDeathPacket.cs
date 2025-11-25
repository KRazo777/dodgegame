using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PlayerDeathPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.PlayerDeath;
        
        public string UniqueId { get; private set; }
        public string KillerUniqueId { get; private set; }

        public PlayerDeathPacket()
        {
        }

        public PlayerDeathPacket(string uniqueId, string killerUniqueId)
        {
            UniqueId = uniqueId;
            KillerUniqueId = killerUniqueId;
        }

        public override void Deserialize(Message message)
        {
            UniqueId = message.GetString();
            KillerUniqueId = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(UniqueId);
            message.AddString(KillerUniqueId);
            return message;
        }

        public void Process(Client client)
        {
            var room = client.User?.Player?.GameRoom;
            if (room == null) return;

            room.Players[UniqueId].IsAlive = false;
            room.Players[KillerUniqueId].Kills++;
        }
    }
}