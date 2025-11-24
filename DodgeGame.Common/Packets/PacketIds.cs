namespace DodgeGame.Common.Packets
{
    public static class PacketIds
    {

        public enum Serverbound : ushort
        {
            Handshake = 1,
            Ping = 2,
            JoinGameRequest = 3,
            Movement = 4,
            GameList = 5,
            GameJoin = 6,
            ClientAuth = 7,
            CreateRoom = 8
        }

        public enum Clientbound : ushort
        {
            Handshake = 1001,
            Pong = 1002,
            JoinGameConfirmed = 1003,
            PlayerDetails = 1004,
            SpawnPlayer = 1005,
            Movement = 1006,
            GameList = 1007,
            ClientAuth = 1008,
            CreatedRoom = 1009
        }
    }
}
