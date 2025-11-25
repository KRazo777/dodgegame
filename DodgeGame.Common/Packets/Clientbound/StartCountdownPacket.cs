using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class StartCountdownPacket : Packet
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.StartCountdown; 

        public StartCountdownPacket() { }
        
        public override void Deserialize(Message message) { }
        
        public override Message Serialize() 
        {
            // ensures countdown signal is received
            return Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.StartCountdown);
        }
        
        public void Process(Client client) 
        { 
            // Client logic will handle this packet in the ConnectionHandler
        }
    }
}