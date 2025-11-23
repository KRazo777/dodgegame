using System;
using System.Diagnostics;
using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Serverbound;
using Riptide;

namespace DodgeGame.Client
{
    public class ConnectionHandler
    {
        private readonly PacketHandler _packetHandler;
        private ClientConnection _clientConnection;

        public ConnectionHandler(ClientConnection clientConnection)
        {
            _packetHandler = new PacketHandler();
            _clientConnection = clientConnection;
        }

        public void Connected(object? sender, EventArgs args)
        {
            _clientConnection.Connection.CanQualityDisconnect = false;
            _clientConnection.Client = new Common.Manager.Client(_clientConnection.Connection);
            UnityEngine.Debug.Log("Connected and sent handshake");
            
        }
        
        public void Disconnected(object? sender, DisconnectedEventArgs args)
        {
            _clientConnection.Client = null;
            UnityEngine.Debug.Log("Disconnected from server");
            
        }

        public void HandlePacket(object? sender, MessageReceivedEventArgs args)
        {
            var messageId = args.MessageId;
            var message = args.Message;

            var packet = _packetHandler.CreateClientboundInstance(messageId);
            if (packet == null)
            {
                // throw some message / error
                return;
            }
            
            UnityEngine.Debug.Log("Received packet " + messageId + " from server");

            packet.Deserialize(message);
            if (_clientConnection.Client != null) packet.Process(_clientConnection.Client);
        }
    }
}