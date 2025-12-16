using System.Collections.Generic;
using System.Linq;
using System.Collections;
using DodgeGame.Common.Game;
using TMPro;
using UnityEngine;
using DodgeGame.Common.Packets.Serverbound;
using UnityEngine.UI;

public class GameListRenderer : MonoBehaviour
{
    public GameObject GameListingPrefab;
    private ServerConnection _serverConnection;

    private List<GameRoom> _shownRooms = new();
    private List<GameObject> _gameListObjects = new();
    
    private RoomJoinHandler _roomJoinHandler;
    

    [SerializeField] private float RefreshGameListSecondsInterval = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _serverConnection = GameObject.Find("NetworkManager").GetComponent<ServerConnection>();
        _roomJoinHandler = GameObject.Find("NetworkManager").GetComponent<RoomJoinHandler>();
        _shownRooms = _serverConnection.ClientConnection.ConnectionHandler.FoundRooms.ToList();
        Debug.Log(_shownRooms.Count);
        RenderGameRooms();;

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("TEST: Manually adding a mock room...");
            
            var mockRoom = new GameRoom("host_123", "MOCK-ROOM", "Test User");
            
            mockRoom.Players.Add("player_1", new Player("player_1", "Test User", EntityType.Player));

            _shownRooms.Add(mockRoom);
            RenderGameRooms();
        }
    }

    private void RenderGameRooms()
    {
        foreach (var gameListObject in _gameListObjects)
        {
            Destroy(gameListObject);
        }
        _gameListObjects.Clear();
        double x = -4;
        double y = 110;
        
        foreach (var room in _shownRooms)
        {
            var roomObj = GameObject.Instantiate(GameListingPrefab, gameObject.transform);
            var button = roomObj.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                _serverConnection.ClientConnection.SendToServer(new JoinGamePacket(room.RoomId));
            });
            roomObj.transform.Find("RoomId").GetComponent<TMP_Text>().text = room.RoomId;
            roomObj.transform.Find("OwnerName").GetComponent<TMP_Text>().text = room.OwnerName;
            roomObj.transform.Find("PlayerCount").GetComponent<TMP_Text>().text = room.Players.Count.ToString();
            
            roomObj.transform.localPosition = new Vector3((float)x, (float)y, 0);
            _gameListObjects.Add(roomObj);
            
            y -= 90;
        }
    }

}