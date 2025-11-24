using System;
using DodgeGame.Client;
using DodgeGame.Common.Packets.Serverbound;
using TMPro;
using UnityEngine;

public class ServerConnection : MonoBehaviour
{
    public ClientConnection _clientConnection;
    public readonly string UniqueId = Guid.NewGuid().ToString();
    
    public GameObject PlayButton;
    
    
    private GameObject _tokenInput;
    public ClientConnection ClientConnection => _clientConnection;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tokenInput = GameObject.Find("TokenInput");
        
        _clientConnection = new ClientConnection();
        _clientConnection.Connect("127.0.0.1:2442");
        _clientConnection.ConnectionHandler.OnAuthSuccess = () =>
        {
            GameObject.Find("Username").GetComponent<TMP_Text>().text = _clientConnection.Client.User.Username;
            _tokenInput.SetActive(false);
            PlayButton.SetActive(true);
        };
        _clientConnection.RiptideClient.Connected += (sender, args) =>
        {
            Debug.Log("Connected");
            // _clientConnection.SendToServer(new HandshakePacket(UniqueId, "TestUsername",
            //     DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
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
