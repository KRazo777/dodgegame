using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class PlayerAddedPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.PlayerAdded;

        public string RoomId { get; private set; }
        public Player Player { get; private set; }

        public PlayerAddedPacket()
        {
            
        }
        public PlayerAddedPacket(string roomId, Player player)
        {
            RoomId = roomId;
            Player = player;
        }

        public override void Deserialize(Message message)
        {
            RoomId = message.GetString();
            Player = Player.Deserialize(message);
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(RoomId);
            Player.Serialize(message);
            return message;
        }

        public void Process(Client client)
        {
            if (client.User != null && client.User.Player != null && client.User.Player.GameRoom != null)
            {
                client.User.Player.GameRoom.Players.TryAdd(Player.Id, Player);
            }
        }
    }
}