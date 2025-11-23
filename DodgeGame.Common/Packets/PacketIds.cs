namespace DodgeGame.Common.Packets
{
    public static class PacketIds
    {
        public static class Serverbound
        {
            public const ushort Handshake = 1;
            public const ushort Ping = 2;
            public const ushort JoinGameRequest = 3;
            public const ushort Movement = 4;
        }

        public static class Clientbound
        {
            public const ushort Handshake = 1001;
            public const ushort Pong = 1002;
            public const ushort JoinGameConfirmed = 1003;
            public const ushort PlayerDetails = 1004;
            public const ushort SpawnPlayer = 1005;
            public const ushort Movement = 1006;
        }
    }
}
