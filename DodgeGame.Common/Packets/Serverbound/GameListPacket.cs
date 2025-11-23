using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class GameListPacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.GameList;

        public GameListPacket()
        {
        }

        public override void Deserialize(Message message)
        {
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.GameList);
            return message;
        }
        
        public override void Process(Client client)
        {
        }
    }
}