using DodgeGame.Common.Packets.Serverbound;
using DodgeGame.Common.Game; 
using UnityEngine;
using TMPro;
using System;

public class CreateGameHandler : MonoBehaviour
{
    public TMP_InputField roomNameInput;
    public TMP_InputField passwordInput;
    
    [Header("UI Transition")]
    public GameObject createRoomPage; 
    public GameObject lobbyPage; 
    
    private ServerConnection _serverConnection;
    private RoomJoinHandler _roomJoinHandler; 
    

    void Awake()
    {
        GameObject networkManager = GameObject.Find("NetworkManager");
        if (networkManager != null)
        {
            _serverConnection = networkManager.GetComponent<ServerConnection>();
        }
        else
        {
            Debug.LogError("FATAL: NetworkManager object not found. CreateGameHandler cannot send requests.");
        }
        
        _roomJoinHandler = FindObjectOfType<RoomJoinHandler>(); 
        
        if (_roomJoinHandler == null)
        {
            Debug.LogError("FATAL: RoomJoinHandler not found. Lobby display will fail.");
        }
    }

    public void RequestHostGame()
    {
        if (_serverConnection == null || _roomJoinHandler == null)
        {
            Debug.LogError("FATAL: Dependencies not initialized. Cannot host game.");
            return;
        }

        string hostId = _serverConnection.UniqueId;
        string roomName = (roomNameInput != null && !string.IsNullOrWhiteSpace(roomNameInput.text)) 
                          ? roomNameInput.text 
                          : $"{hostId}'s Room"; 
        
        string password = passwordInput != null ? passwordInput.text : "";
        bool isPrivate = !string.IsNullOrEmpty(password);
        

        _serverConnection.ClientConnection.SendToServer(
            new CreateGameRequestPacket(
                hostId, 
                roomName,
                password,
                isPrivate
            )
        );
        
        Debug.Log($"Sent request to HOST room: {roomName} (Private: {isPrivate})");

        if (createRoomPage != null) createRoomPage.SetActive(false); 
        if (lobbyPage != null) lobbyPage.SetActive(true); 
        
        ClearInputs();
        
        GameRoom temporaryRoom = new GameRoom(
            hostId, 
            Guid.NewGuid().ToString(), 
            roomName
        );
        
        temporaryRoom.RoomPassword = password;
        temporaryRoom.IsPrivate = isPrivate;

        _roomJoinHandler.UpdateLobbyDisplay(temporaryRoom);
    }

    // helper function to clear inputs after return
    private void ClearInputs()
    {
        if (roomNameInput != null)
        {
            roomNameInput.text = "";
            roomNameInput.DeactivateInputField(); 
        }

        if (passwordInput != null)
        {
            passwordInput.text = "";
            passwordInput.DeactivateInputField();
        }
    }
}
