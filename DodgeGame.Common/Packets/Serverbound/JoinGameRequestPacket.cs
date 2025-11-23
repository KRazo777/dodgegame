using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class JoinGameRequestPacket : Packet
    {
        public override ushort Id => PacketIds.Serverbound.JoinGameRequest;
        public ushort ClientId { get; private set; }
        public string GameRoomId { get; private set; } = string.Empty;

        public JoinGameRequestPacket()
        {
        }

        public JoinGameRequestPacket(ushort clientId, string gameRoomId)
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
            var message = Message.Create(MessageSendMode.Reliable, PacketIds.Serverbound.JoinGameRequest);
            message.AddUShort(ClientId);
            message.AddString(GameRoomId);
            return message;
        }

        public override void Process(Client client)
        {
            // Processing will be handled in server logic where this packet is consumed.
        }
    }
}
