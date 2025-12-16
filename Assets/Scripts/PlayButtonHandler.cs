using DodgeGame.Common.Packets.Serverbound;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonHandler : MonoBehaviour
{
    private ServerConnection _serverConnection;
    private Button _button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(RequestGameList);
            
        _serverConnection = GameObject.Find("NetworkManager").GetComponent<ServerConnection>();
    }

    private void RequestGameList()
    {
        _serverConnection.ClientConnection.SendToServer(new RequestGameListPacket());
    }
}