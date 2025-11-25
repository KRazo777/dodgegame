using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class StartGamePacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.StartGame;
        
        public GameRoom GameRoom { get; private set; }
        
        public StartGamePacket()
        {}

        public StartGamePacket(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        public override void Deserialize(Message message)
        {
            GameRoom = GameRoom.Deserialize(message);
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            GameRoom.Serialize(message);
            return message;
        }

        public void Process(Client client)
        {
            client.User.Player.GameRoom = GameRoom;
        }
    }
}