using System.Collections.Generic;
using System.Linq;
using System.Collections;
using DodgeGame.Common.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DodgeGame.Common.Packets.Serverbound; // Needed for RequestGameListPacket

public class GameListRenderer : MonoBehaviour
{
    public GameObject GameListingPrefab;
    private ServerConnection _serverConnection;

    private List<GameObject> _spawnedButtons = new();
    private string _lastSignature = "";

    void Start()
    {
        _serverConnection = GameObject.Find("NetworkManager").GetComponent<ServerConnection>();
        
        if (GameListingPrefab == null) Debug.LogError("GameListingPrefab is missing in Inspector");
        
        StartCoroutine(RefreshGameListRoutine());
    }

    IEnumerator RefreshGameListRoutine()
    {
        while (true)
        {
            // Ask Server for the list
            _serverConnection.ClientConnection.SendToServer(new RequestGameListPacket());
            yield return new WaitForSeconds(2f);
        }
    }

    void Update()
    {
        var handler = _serverConnection.ClientConnection.ConnectionHandler;
        var rooms = handler.FoundRooms;

        // debugging, Print count every frame so we KNOW if the UI sees the data
        if (rooms.Count > 0) 
        {
            // Only print this once every 60 frames to avoid spamming too hard
            if (Time.frameCount % 60 == 0) 
                Debug.Log($" UI WATCHER: I see {rooms.Count} rooms in memory.");
        }

        // Check for changes
        string newSignature = string.Join(",", rooms.Select(r => r.RoomId));
        
        if (_lastSignature != newSignature)
        {
            Debug.Log($" UI updating! Old: '{_lastSignature}' -> New: '{newSignature}'");
            _lastSignature = newSignature;
            Render(rooms);
        }
    }

    void Render(List<GameRoom> rooms)
    {
        // Wipe old buttons
        foreach (var btn in _spawnedButtons) Destroy(btn);
        _spawnedButtons.Clear();

        float y = 120f; // Start position

        foreach (var room in rooms)
        {
            Debug.Log($" Spawning Button for: {room.RoomId}");
            
            GameObject obj = Instantiate(GameListingPrefab, transform);
            
            obj.transform.localPosition = new Vector3(0, y, 0);
            obj.transform.localScale = Vector3.one; 
            obj.transform.localRotation = Quaternion.identity;
            
        
            SetText(obj, "RoomId", room.RoomId);
            SetText(obj, "OwnerName", room.OwnerName);
            SetText(obj, "PlayerCount", room.Players.Count.ToString());

            // Add Click Listener
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"Joining {room.RoomId}...");
                _serverConnection.ClientConnection.SendToServer(new JoinGamePacket(room.RoomId));
            });

            _spawnedButtons.Add(obj);
            y -= 90f; // Move down
        }
    }

    // Helper to safely set text
    void SetText(GameObject parent, string childName, string content)
    {
        var child = parent.transform.Find(childName);
        if (child != null) child.GetComponent<TMP_Text>().text = content;
        else Debug.LogWarning($"Prefab Warning: Could not find child named '{childName}'");
    }
}