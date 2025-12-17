using System.Collections.Generic;
using System.Linq;
using System.Collections;
using DodgeGame.Common.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DodgeGame.Common.Packets.Serverbound; 

public class GameListRenderer : MonoBehaviour
{
    public GameObject GameListingPrefab;
    
    public Transform listContainer; 

    private ServerConnection _serverConnection;
    private List<GameObject> _spawnedButtons = new();
    private string _lastSignature = "";

    void Start()
    {
        _serverConnection = GameObject.Find("NetworkManager").GetComponent<ServerConnection>();

        if (listContainer == null) 
        {
            Debug.LogWarning(" UI WARNING: listContainer is not assigned! Using 'transform', which might delete your background!");
            listContainer = transform;
        }

        StartCoroutine(RefreshGameListRoutine());
    }

    IEnumerator RefreshGameListRoutine()
    {
        while (true)
        {
            _serverConnection.ClientConnection.SendToServer(new RequestGameListPacket());
            yield return new WaitForSeconds(2f);
        }
    }

    void Update()
    {
        var handler = _serverConnection.ClientConnection.ConnectionHandler;
        var rooms = handler.FoundRooms;

        string newSignature = string.Join(",", rooms.Select(r => r.RoomId));
        
        if (_lastSignature != newSignature || rooms.Count == 0 && _spawnedButtons.Count > 0)
        {
            _lastSignature = newSignature;
            Render(rooms);
        }
    }

    void Render(List<GameRoom> rooms)
    {
        if (!listContainer.gameObject.activeSelf) 
        {
            listContainer.gameObject.SetActive(true);
        }

        foreach (Transform child in listContainer)
        {
            Destroy(child.gameObject);
        }
        _spawnedButtons.Clear();

        if (rooms.Count == 0) return;

        float y = 200f; 

        foreach (var room in rooms)
        {
            GameObject obj = Instantiate(GameListingPrefab, listContainer);
            
            // Reset position to be relative to the container
            obj.transform.localPosition = new Vector3(0, y, 0);
            obj.transform.localScale = Vector3.one; 
            
            SetText(obj, "RoomId", room.RoomId);
            SetText(obj, "OwnerName", room.OwnerName);
            SetText(obj, "PlayerCount", room.Players.Count.ToString());

            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                _serverConnection.ClientConnection.SendToServer(new JoinGamePacket(room.RoomId));
            });

            _spawnedButtons.Add(obj);
            y -= 90f;
        }
    }

    void SetText(GameObject parent, string childName, string content)
    {
        var child = parent.transform.Find(childName);
        if (child != null) child.GetComponent<TMP_Text>().text = content;
    }
}