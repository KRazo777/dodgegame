using Riptide;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PlayerJoinPacket : Packet
    {
       
        public override ushort Id => PacketIds.Clientbound.PlayerJoin; 

        public string UniqueId { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        
        // include a flag if the joining player is the Host
        // public bool IsHost { get; private set; } 

        public PlayerJoinPacket()
        {
        }

        public PlayerJoinPacket(string uniqueId, string name)
        {
            UniqueId = uniqueId;
            Name = name;
        }

        public override void Deserialize(Message message)
        {
            UniqueId = message.GetString();
            Name = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.PlayerJoin);
            message.AddString(UniqueId);
            message.AddString(Name);
            return message;
        }

        public override void Process(Manager.Client client)
        {
            // add to the player count in the LobbyPage and put them in the LobbyPageRoom
        }
    }
}