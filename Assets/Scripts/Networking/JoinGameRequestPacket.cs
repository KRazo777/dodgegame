using Riptide; 
using DodgeGame.Common.Packets;
using Client = DodgeGame.Common.Manager.Client; 

// packet tells the server that the client wants to join an existing room
namespace DodgeGame.Common.Packets.Serverbound
{

    public class JoinGameRequestPacket : Packet
    {
        public override ushort Id => 4; 

        // server expects the network ID I think
        public readonly ushort ClientNetworkId; 
        public readonly string RoomId;
        
        public JoinGameRequestPacket(ushort clientNetworkId, string roomId)
        {
            ClientNetworkId = clientNetworkId;
            RoomId = roomId;
        }

        // used to write the data into the network message buffer
        public override Message Serialize()
        {
            Message message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddUShort(ClientNetworkId);
            message.AddString(RoomId);
            return message;
        }
        
        public override void Deserialize(Message message) { /* Client doesnts deserialize outgoing packets */ }
       
    }
}