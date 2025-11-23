using Riptide;

namespace DodgeGame.Common.Packets.Clientbound
{
    public class JoinGameConfirmedPacket : Packet
    {
        public override ushort Id => PacketIds.Clientbound.JoinGameConfirmed;
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
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Clientbound.JoinGameConfirmed);
            message.AddUShort(ClientId);
            message.AddString(GameRoomId);
            return message;
        }

        public override void Process(Manager.Client client)
        {
            // Clientbound packets are handled on the client application.
        }
    }
}
