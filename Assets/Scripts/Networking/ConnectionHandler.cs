using System;
using System.Collections.Generic;
using DodgeGame.Common.Game;
using DodgeGame.Common.Packets;
using Riptide;
using TMPro;
using UnityEngine;
using GameListPacket = DodgeGame.Common.Packets.Clientbound.GameListPacket;

namespace DodgeGame.Client
{
    public class ConnectionHandler
    {
        private readonly PacketHandler _packetHandler;
        private readonly ClientConnection _clientConnection;
        public readonly List<GameRoom> FoundRooms = new List<GameRoom>();
        
        public Action OnAuthSuccess { get; set; }

        public ConnectionHandler(ClientConnection clientConnection)
        {
            _packetHandler = new PacketHandler();
            _clientConnection = clientConnection;

            
        }

        public void Connected(object? sender, EventArgs args)
        {
            _clientConnection.Connection.CanQualityDisconnect = false;
            _clientConnection.Client = new Common.Manager.Client(_clientConnection.Connection);
            Debug.Log("Connected and sent handshake");
        }

        public void Disconnected(object? sender, DisconnectedEventArgs args)
        {
            _clientConnection.Client = null;
            Debug.Log("Disconnected from server");
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

            Debug.Log("Received packet " + messageId + " from server");

            packet.Deserialize(message);
            if (_clientConnection.Client != null) packet.Process(_clientConnection.Client);

            if (messageId == PacketIds.Clientbound.GameList)
            {
                var gameList = (GameListPacket)packet;
                FoundRooms.Clear();
                FoundRooms.AddRange(gameList.GameRooms);
                return;
            }

            if (messageId == PacketIds.Clientbound.ClientAuth)
            {
                if (OnAuthSuccess != null) OnAuthSuccess();
                
            }
        }
    }
}