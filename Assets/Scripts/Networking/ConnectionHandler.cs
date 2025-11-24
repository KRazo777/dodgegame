using System;
using System.Collections.Generic;
using DodgeGame.Common.Game;
using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Clientbound;
using Riptide;
using UnityEngine;

namespace DodgeGame.Client
{
    public class ConnectionHandler
    {
        private readonly PacketHandler _packetHandler;
        private readonly ClientConnection _clientConnection;
        private readonly RoomJoinHandler _roomJoinHandler;
        
        public readonly List<GameRoom> FoundRooms = new List<GameRoom>();
        
        public Action OnAuthSuccess { get; set; }

        public ConnectionHandler(ClientConnection clientConnection)
        {
            _packetHandler = new PacketHandler();
            _clientConnection = clientConnection;
            
            _packetHandler.RegisterClientbound<HandshakePacket>();
            _packetHandler.RegisterClientbound<PongPacket>();
            _packetHandler.RegisterClientbound<JoinGameConfirmedPacket>();
            _packetHandler.RegisterClientbound<PlayerDetailsPacket>();
            _packetHandler.RegisterClientbound<SpawnPlayerPacket>();
            _packetHandler.RegisterClientbound<MovementPacket>();
            _packetHandler.RegisterClientbound<GameListPacket>();
            _packetHandler.RegisterClientbound<ClientAuthenticatedPacket>();
            _packetHandler.RegisterClientbound<CreatedRoomPacket>();

            _roomJoinHandler = GameObject.FindWithTag("NetworkManager").GetComponent<RoomJoinHandler>();
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
            var messageId = (PacketIds.Clientbound) args.MessageId;
            var message = args.Message;

            var packet = _packetHandler.CreateClientboundInstance((ushort)messageId);
            if (packet == null)
            {
                // throw some message / error
                return;
            }

            Debug.Log("Received packet " + messageId + " from server");

            packet.Deserialize(message);
            if (_clientConnection.Client != null) ((IClientPacket)packet).Process(_clientConnection.Client);

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

            if (messageId == PacketIds.Clientbound.CreatedRoom)
            {
                var createdRoom = (CreatedRoomPacket)packet;
                Debug.Log("Created room with id " + createdRoom.GameRoom.RoomId + " with owner " + createdRoom.GameRoom.OwnerName);
                
                _roomJoinHandler.UpdateLobbyDisplay(createdRoom.GameRoom);
            }
        }
    }
}