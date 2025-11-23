using System;
using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Serverbound;
using Riptide;
using Riptide.Utils;

namespace DodgeGame.Client
{
    public class ClientConnection
    {
        private readonly Riptide.Client _riptideClient;
        private readonly ConnectionHandler _connectionHandler;
        private readonly TimeSpan _pingInterval = TimeSpan.FromSeconds(1);
        private DateTime _lastPingSentUtc = DateTime.MinValue;

        public Common.Manager.Client? Client;

        public Riptide.Client RiptideClient => _riptideClient;
        public Connection Connection => _riptideClient.Connection;

        public ClientConnection()
        {
            RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);
            _riptideClient = new Riptide.Client();
            _connectionHandler = new ConnectionHandler(this);

            _riptideClient.Connected += _connectionHandler.Connected;
            _riptideClient.Disconnected += _connectionHandler.Disconnected;
            _riptideClient.MessageReceived += _connectionHandler.HandlePacket;
        }

        public void Update()
        {
            _riptideClient.Update();
            if (Client != null)
            {
                SendPingIfDue();
            }
        }

        public void Connect(string endpoint)
        {
            _riptideClient.Connect(endpoint, 5, 0, null, false);
        }

        public void Disconnect()
        {
            _riptideClient.Disconnect();
            _lastPingSentUtc = DateTime.MinValue;
        }

        public void SendToServer(Packet packet)
        {
            _riptideClient.Send(packet.Serialize());
        }

        private void SendPingIfDue()
        {
            var now = DateTime.UtcNow;
            if (now - _lastPingSentUtc < _pingInterval)
                return;

            _lastPingSentUtc = now;
            SendToServer(new PingPacket(now.Ticks));
        }
    }
}
