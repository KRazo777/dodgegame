using System;
using System.Collections.Generic;
using DodgeGame.Common.Game;
using DodgeGame.Common.Packets;
using DodgeGame.Common.Packets.Clientbound;
using Newtonsoft.Json;
using Riptide;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace DodgeGame.Client
{
    public class ConnectionHandler
    {
        private readonly PacketHandler _packetHandler;
        private readonly ClientConnection _clientConnection;
        private readonly RoomJoinHandler _roomJoinHandler;
        private readonly PrefabHolder _prefabHolder;

        public readonly List<GameRoom> FoundRooms = new List<GameRoom>();

        public Action OnAuthSuccess { get; set; }

        public ConnectionHandler(ClientConnection clientConnection)
        {
            _packetHandler = new PacketHandler();
            _clientConnection = clientConnection;

            _packetHandler.RegisterClientbound<HandshakePacket>();
            _packetHandler.RegisterClientbound<PongPacket>();
            _packetHandler.RegisterClientbound<JoinedGamePacket>();
            _packetHandler.RegisterClientbound<PlayerDetailsPacket>();
            _packetHandler.RegisterClientbound<PlayerAddedPacket>();
            _packetHandler.RegisterClientbound<MovementPacket>();
            _packetHandler.RegisterClientbound<GameListPacket>();
            _packetHandler.RegisterClientbound<ClientAuthenticatedPacket>();
            _packetHandler.RegisterClientbound<CreatedRoomPacket>();
            _packetHandler.RegisterClientbound<StartGamePacket>();

            _roomJoinHandler = GameObject.FindWithTag("NetworkManager").GetComponent<RoomJoinHandler>();
            _prefabHolder = GameObject.FindWithTag("NetworkManager").GetComponent<PrefabHolder>();

            SceneManager.sceneLoaded += SceneLoaded;
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
            _clientConnection.Connect("127.0.0.1:2442");
            Debug.Log("Disconnected from server");
        }

        public void HandlePacket(object? sender, MessageReceivedEventArgs args)
        {
            var messageId = (PacketIds.Clientbound)args.MessageId;
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
                Debug.Log($"Got {gameList.GameRooms} gamerooms");
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
                Debug.Log("Created room with id " + createdRoom.GameRoom.RoomId + " with owner " +
                          createdRoom.GameRoom.OwnerName);

                _roomJoinHandler.OpenLobbyScreen();
                _roomJoinHandler.UpdateLobbyDisplay(createdRoom.GameRoom);
            }

            if (messageId == PacketIds.Clientbound.JoinedGame)
            {
                var joinedGame = (JoinedGamePacket)packet;
                Debug.Log("Joined game with id " + joinedGame.GameRoom.RoomId);

                Debug.Log("Given player " + _clientConnection.Client.User.Player.Id + " name " +
                          _clientConnection.Client.User.Player.Name);
                Debug.Log(joinedGame.GameRoom.Players.Count + " players in room");

                Debug.Log(JsonConvert.SerializeObject(joinedGame.GameRoom));

                foreach (var playersValue in joinedGame.GameRoom.Players.Values)
                {
                    Debug.Log("Player ID: " + playersValue.Id + " Name: " + playersValue.Name);
                }

                _roomJoinHandler.OpenLobbyScreen();
                _roomJoinHandler.UpdateLobbyDisplay(joinedGame.GameRoom);
            }

            if (messageId == PacketIds.Clientbound.PlayerAdded)
            {
                var playerAdded = (PlayerAddedPacket)packet;
                Debug.Log(playerAdded.Player.Name + " joined the game!");
                _roomJoinHandler.UpdateLobbyDisplay(_clientConnection.Client.User.Player.GameRoom);
            }

            if (messageId == PacketIds.Clientbound.StartGame)
            {
                var startGame = (StartGamePacket)packet;
                SceneManager.LoadScene("GameScene");

                var room = startGame.GameRoom;
                Debug.Log("Starting game with room id " + room.RoomId);
                Debug.Log(JsonConvert.SerializeObject(room));
            }

            if (messageId == PacketIds.Clientbound.Movement)
            {
                var movement = (MovementPacket)packet;
                Debug.Log(GameObject.Find(movement.UniqueId));
                Debug.Log(movement.X + " " + movement.Y);
                GameObject.Find(movement.UniqueId).GetComponent<Rigidbody2D>().linearVelocity =
                    new Vector2(movement.X, movement.Y) * 1f;
            }
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GameScene")
            {
                Debug.Log("Game scene loaded");
                foreach (var player in _clientConnection.Client.User.Player.GameRoom.Players.Values)
                {
                    Debug.Log("Player ID: " + player.Id + " Name: " + player.Name);
                    Debug.Log(JsonConvert.SerializeObject(player.Position));
                    var playerObject = Object.Instantiate(_prefabHolder.playerPrefab);
                    playerObject.transform.position = new Vector3(player.Position.X, player.Position.Y, -1);
                    if (player.Id.Equals(_clientConnection.Client.User.UniqueId))
                    {
                        playerObject.AddComponent<CharacterController>();
                        GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().player =
                            playerObject.transform;
                    }
                    else
                    {
                        Object.Destroy(playerObject.transform.Find("RotatePoint").gameObject);
                    }

                    playerObject.name = player.Id;
                    playerObject.tag = "Player";
                }
            }
        }
    }
}