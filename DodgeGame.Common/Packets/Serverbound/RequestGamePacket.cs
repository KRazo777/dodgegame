using DodgeGame.Common.Game;
using DodgeGame.Common.Packets.Clientbound;
using Riptide;
using System.Linq;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class RequestGameListPacket : Packet, IServerPacket
    {
        // Link it to the ID you just made
        public override ushort Id => (ushort)PacketIds.Serverbound.RequestGameList;

        public override void Deserialize(Message message) { /* Empty - No data needed */ }
        public override Message Serialize() { return Message.Create(MessageSendMode.Reliable, Id); }

        public void Process(IGameServer gameServer, Client client)
        {
            var rooms = gameServer.GameRooms.Values.ToArray();
            
            client.SendPacket(new GameListPacket(rooms));
        }
    }
}