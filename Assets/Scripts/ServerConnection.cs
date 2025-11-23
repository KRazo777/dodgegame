using System;
using DodgeGame.Client;
using DodgeGame.Common.Packets.Serverbound;
using UnityEngine;

public class ServerConnection : MonoBehaviour
{
    public ClientConnection _clientConnection;
    public readonly string UniqueId = Guid.NewGuid().ToString();
    public ClientConnection ClientConnection => _clientConnection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _clientConnection = new ClientConnection();
        _clientConnection.Connect("127.0.0.1:2442");
        _clientConnection.RiptideClient.Connected += (sender, args) =>
        {
            Debug.Log("Connected");
            _clientConnection.SendToServer(new HandshakePacket(UniqueId, "TestUsername",
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (_clientConnection != null)
        {
            _clientConnection.Update();
        }
    }

    private void OnApplicationQuit()
    {
        _clientConnection.Disconnect();
    }
}
