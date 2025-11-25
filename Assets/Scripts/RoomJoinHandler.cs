using DodgeGame.Common.Game;
using DodgeGame.Common.Packets.Serverbound;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
using System;

public class RoomJoinHandler : MonoBehaviour
{
    // ASSIGNED IN INSPECTOR 
    [Header("UI Objects")]
    public GameObject gamesPage; // The page to hide (GamesPage)
    public GameObject lobbyPage; // The page to show (LobbyPage)
    public TMP_Text playerListText; // The text element inside LobbyPage to show names

    [Header("Game Scene Config")]
    public string gameSceneName = "GameScene";

    private ServerConnection _serverConnection;
    private GameRoom _roomToJoin;
    
    void Awake()
    {
        GameObject networkManager = GameObject.Find("NetworkManager");
        if (networkManager != null)
        {
            _serverConnection = networkManager.GetComponent<ServerConnection>();
        }
        else
        {
            Debug.LogError("FATAL: NetworkManager object not found. RoomJoinHandler cannot function.");
        }
    }
    

    public void OpenLobbyScreen()
    {
        // Hide the current menu and show the Lobby screen
        if (gamesPage != null) gamesPage.SetActive(false);
        if (lobbyPage != null) lobbyPage.SetActive(true);

        if (playerListText != null && _roomToJoin != null)
        {
             playerListText.text = $"Attempting to join room: {_roomToJoin.OwnerName}\n\nWaiting for server confirmation...";
        }
    }
    
    public void UpdateLobbyDisplay(GameRoom updatedRoom)
    {
        _roomToJoin = updatedRoom;
        
        string players = "";
        
        // Render player names from the updated room list
        foreach (var player in updatedRoom.Players.Values)
        {
            //  assumes Player.Id and GameRoom.HostUniqueId are comparable
            players += player.Name + (player.Id == updatedRoom.HostUniqueId ? " (HOST)" : "") + "\n";
        }

        // update UI
        playerListText.text = $"Room: {updatedRoom.OwnerName}\nPlayers: {updatedRoom.Players.Count}/4\n\n{players}";
        
    }
}
