using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class JoinedGamePacket : Packet, IClientPacket
    {
        private GameRoom _gameRoom;
        private Player _player;
        
        public GameRoom GameRoom => _gameRoom;

        public JoinedGamePacket()
        {}
        
        public JoinedGamePacket(GameRoom gameRoom, Player player)
        {
            _gameRoom = gameRoom;
            _player = player;
        }

        public override ushort Id => (ushort)PacketIds.Clientbound.JoinedGame;
        public override void Deserialize(Message message)
        {
            _gameRoom = GameRoom.Deserialize(message);
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            _gameRoom.Serialize(message);
            return message;
        }

        public void Process(Client client)
        {
            client.User.Player = _gameRoom.Players[client.User.UniqueId];
            client.User.Player.GameRoom = _gameRoom;
        }
    }
}