using System.Collections.Generic;
using System.Linq;
using System.Collections;
using DodgeGame.Common.Game;
using TMPro;
using UnityEngine;
using DodgeGame.Common.Packets.Serverbound;

public class GameListRenderer : MonoBehaviour
{
    private ServerConnection _serverConnection;

    private List<GameRoom> _shownRooms = new List<GameRoom>();

    private GameObject _gameRoomObject;

    [SerializeField] private float RefreshGameListSecondsInterval = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _serverConnection = GameObject.Find("NetworkManager").GetComponent<ServerConnection>();
        _shownRooms = _serverConnection.ClientConnection.ConnectionHandler.FoundRooms.ToList();
        Debug.Log(_shownRooms.Count);

        _gameRoomObject = GameObject.Find("Game");
        updateObject();

        StartCoroutine(RefreshGameListRoutine());
    }

    IEnumerator RefreshGameListRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(RefreshGameListSecondsInterval);

            Debug.Log("REFRESHING GAME LIST");
            RequestGameList();

        }
    }

    void RequestGameList()
    {
        _serverConnection.ClientConnection.SendToServer(new GameListPacket());
    }

    // Update is called once per frame
    void Update()
    {
        if (_shownRooms.Count != _serverConnection.ClientConnection.ConnectionHandler.FoundRooms.Count)
        {
            _shownRooms = _serverConnection.ClientConnection.ConnectionHandler.FoundRooms.ToList();
            Debug.Log(_shownRooms.Count);
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