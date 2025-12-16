namespace DodgeGame.Common.Packets
{
    public static class PacketIds
    {

        public enum Serverbound : ushort
        {
            Handshake = 1,
            Ping = 2,
            Movement = 3,
            GameList = 4,
            JoinGame = 5,
            ClientAuth = 6,
            CreateRoom = 7,
            BulletHit = 8,
            BulletFired = 9,
            RequestGameList = 10
        }

        public enum Clientbound : ushort
        {
            Handshake = 1001,
            Pong = 1002,
            PlayerDetails = 1003,
            Movement = 1004,
            GameList = 1005,
            ClientAuth = 1006,
            CreatedRoom = 1007,
            JoinedGame = 1008,
            PlayerAdded = 1009,
            StartGame = 1010,
            StartCountdown = 1011,
            PlayerDeath = 1012,
            BulletFired = 1013,
        }
    }
}
