using DodgeGame.Common.Game;
using Riptide;
using Client = DodgeGame.Common.Manager.Client;

namespace DodgeGame.Common.Packets.Serverbound
{
    public class LeaveGamePacket : Packet, IServerPacket
    {
        public override ushort Id => (ushort)PacketIds.Serverbound.LeaveGame;

        public override void Deserialize(Message message) { }
        public override Message Serialize() { return Message.Create(MessageSendMode.Reliable, Id); }

        public void Process(IGameServer gameServer, Client client)
        {
            if (client.User == null || client.User.Player == null) return;

            var room = client.User.Player.GameRoom;
            if (room != null)
            {
                //  Remove Player from Room
                if (room.Players.ContainsKey(client.User.UniqueId))
                {
                    room.Players.Remove(client.User.UniqueId);
                    System.Console.WriteLine($"[GAME] {client.User.Username} left Room {room.RoomId} manually.");
                }

                // Destroy Room if empty
                if (room.Players.Count == 0)
                {
                    if (gameServer.GameRooms.TryRemove(room.RoomId, out _))
                    {
                        System.Console.WriteLine($"[GAME] 🗑️ Room {room.RoomId} is empty. Destroyed.");
                    }
                }
                
                client.User.Player.GameRoom = null;
                client.User.Player = null;
            }
        }
    }
}