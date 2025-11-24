using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class JoinGameConfirmedPacket : Packet, IClientPacket
    {
        public override ushort Id => (ushort)PacketIds.Clientbound.JoinGameConfirmed;
        public ushort ClientId { get; private set; }
        public string GameRoomId { get; private set; } = string.Empty;

        public JoinGameConfirmedPacket()
        {
        }

        public JoinGameConfirmedPacket(ushort clientId, string gameRoomId)
        {
            ClientId = clientId;
            GameRoomId = gameRoomId;
        }

        public override void Deserialize(Message message)
        {
            ClientId = message.GetUShort();
            GameRoomId = message.GetString();
        }

        public override Message Serialize()
        {
            var message = Message.Create(MessageSendMode.Reliable, Id);
            message.AddUShort(ClientId);
            message.AddString(GameRoomId);
            return message;
        }

        public void Process(Client client)
        {
            // Clientbound packets are handled on the client application.
        }
    }
}
