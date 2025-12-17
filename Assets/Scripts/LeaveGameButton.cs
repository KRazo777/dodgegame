using DodgeGame.Common.Packets.Serverbound; 
using UnityEngine;
using UnityEngine.UI;

public class LeaveGameButton : MonoBehaviour
{
    private ServerConnection _serverConnection;
    private Button _button;

    void Start()
    {
        var netManager = GameObject.FindGameObjectWithTag("NetworkManager");
        if (netManager) _serverConnection = netManager.GetComponent<ServerConnection>();

        _button = GetComponent<Button>();

        _button.onClick.AddListener(SendLeavePacket);
    }

    void SendLeavePacket()
    {
        Debug.Log("Leaving Game...");

        if (_serverConnection != null && _serverConnection.ClientConnection != null)
        {
            var client = _serverConnection.ClientConnection.Client;
            
            if (client.User != null && client.User.Player != null && client.User.Player.GameRoom != null)
            {
                string myRoomId = client.User.Player.GameRoom.RoomId;
                
                _serverConnection.ClientConnection.ConnectionHandler.FoundRooms
                    .RemoveAll(r => r.RoomId == myRoomId);
            }

            _serverConnection.ClientConnection.SendToServer(new LeaveGamePacket());
        }
    }
}