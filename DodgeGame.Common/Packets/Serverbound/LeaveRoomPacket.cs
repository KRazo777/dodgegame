using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class LeaveRoomPacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.LeaveRoom; 

        public LeaveRoomPacket()
        {
        }

        public override void Deserialize(Message message)
        {
            
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.LeaveRoom);
            return message;
        }

        public override void Process(Client client)
        {
            // Server will receive this, identify the client, remove them 
            // from the room and update the player count.
        }
    }
}