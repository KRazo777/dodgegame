using System.Linq;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class GameListPacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.GameList;

        public GameListPacket()
        {
        }

        public override void Deserialize(Message message)
        {
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            return message;
        }
        
        public void Process(IGameServer gameServer, Client client)
        {
            client.SendPacket(new Clientbound.GameListPacket(gameServer.GameRooms.Values.ToArray()));
        }
    }
}