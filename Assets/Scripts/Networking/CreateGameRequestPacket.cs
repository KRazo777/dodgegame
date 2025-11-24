using Riptide; 
using DodgeGame.Common.Packets;
using Client = DodgeGame.Common.Manager.Client; 

// This packet tells the server to create a new GameRoom.
namespace DodgeGame.Common.Packets.Serverbound
{
    public class CreateGameRequestPacket : Packet
    {
        public override ushort Id => 6; // set Serverbound ID to 6

        public readonly string HostUniqueId;
        public readonly string RoomName;
        public readonly string RoomPassword;
        public readonly bool IsPrivate;
        
        // Constructor called by CreateGameHandler.cs when the button is clicked
        public CreateGameRequestPacket(string hostUniqueId, string roomName, string roomPassword, bool isPrivate)
        {
            HostUniqueId = hostUniqueId;
            RoomName = roomName;
            RoomPassword = roomPassword;
            IsPrivate = isPrivate;
        }

        public override Message Serialize()
        {
            Message message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddString(HostUniqueId);
            message.AddString(RoomName);
            message.AddString(RoomPassword);
            message.AddBool(IsPrivate);
            return message;
        }
        
        // we use the full namespace path to satisfy the compiler.
        public override void Deserialize(Message message) { /* Client doesn't deserialize outgoing packets */ }
    }
}