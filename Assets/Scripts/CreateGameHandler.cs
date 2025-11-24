using DodgeGame.Common.Packets.Serverbound;
using DodgeGame.Common.Game; 
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CreateGameHandler : MonoBehaviour
{
    public GameObject _passwordInputObject;
    
    private TMP_InputField _passwordInput;
    
    [Header("UI Transition")]
    public GameObject createRoomPage; 
    public GameObject lobbyPage; 
    
    private ServerConnection _serverConnection;
    private Button _button;
    

    void Awake()
    {
        Debug.Log(gameObject.name);
        GameObject networkManager = GameObject.Find("NetworkManager");
        if (networkManager != null)
        {
            _serverConnection = networkManager.GetComponent<ServerConnection>();
        }
        else
        {
            Debug.LogError("FATAL: NetworkManager object not found. CreateGameHandler cannot send requests.");
        }
        
        _passwordInput = _passwordInputObject.GetComponent<TMP_InputField>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(RequestHostGame);
        
    }

    void Start()
    {
        
    }

    public void RequestHostGame()
    {
        if (_serverConnection == null)
        {
            Debug.LogError("FATAL: Dependencies not initialized. Cannot host game.");
            return;
        }

        string hostId = _serverConnection.UniqueId;
        
        string password = _passwordInput != null ? _passwordInput.text : "";
        bool isPrivate = !string.IsNullOrEmpty(password);
        

        _serverConnection.ClientConnection.SendToServer(
            new CreateRoomPacket(password ?? "", isPrivate)
        );
        
        Debug.Log($"Sent request to HOST room: (Private: {isPrivate})");

        if (createRoomPage != null) createRoomPage.SetActive(false); 
        if (lobbyPage != null) lobbyPage.SetActive(true); 
        
        ClearInputs();
        
        // GameRoom temporaryRoom = new GameRoom(
        //     hostId, 
        //     Guid.NewGuid().ToString(), 
        //     roomName
        // );
        //
        // temporaryRoom.RoomPassword = password;
        // temporaryRoom.IsPrivate = isPrivate;

        // _roomJoinHandler.UpdateLobbyDisplay(temporaryRoom);
    }

    // helper function to clear inputs after return
    private void ClearInputs()
    {

        if (_passwordInput != null)
        {
            _passwordInput.text = "";
            _passwordInput.DeactivateInputField();
        }
    }
}
