using System.Collections.Generic;
using System.Linq;
using DodgeGame.Common.Game;
using TMPro;
using UnityEngine;

public class GameListRenderer : MonoBehaviour
{
    private ServerConnection _serverConnection;

    private List<GameRoom> _shownRooms = new List<GameRoom>();

    private GameObject _gameRoomObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _serverConnection = GameObject.Find("NetworkManager").GetComponent<ServerConnection>();
        _shownRooms = _serverConnection.ClientConnection.ConnectionHandler.FoundRooms.ToList();
        Debug.Log(_shownRooms.Count);
        
        _gameRoomObject = GameObject.Find("Game");
        updateObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (_shownRooms.Count != _serverConnection.ClientConnection.ConnectionHandler.FoundRooms.Count)
        {
            _shownRooms = _serverConnection.ClientConnection.ConnectionHandler.FoundRooms.ToList();
            updateObject();
        }
    }

    private void updateObject()
    {
        if (_shownRooms.Count > 0)
        {
            Debug.Log(_shownRooms[0].RoomName);
            Debug.Log(_gameRoomObject.transform.Find("RoomName"));
            // foreach (var comp in _gameRoomObject.transform.Find("RoomName").GetComponents<>())
            // {
            //     Debug.Log(comp);
            // }
            Debug.Log(_gameRoomObject.transform.Find("RoomName").GetComponent<TMP_Text>());
            _gameRoomObject.transform.Find("RoomName").GetComponent<TMP_Text>().text = _shownRooms[0].RoomName;
        }
    }
}