using System;
using DodgeGame.Common.Packets.Serverbound;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    
    // Send coordinates 10 times a second (0.1f) to save bandwidth but keep accuracy
    [SerializeField] float sendInterval = 0.1f; 

    private Rigidbody2D rb;
    private Vector2 input;
    private ServerConnection _serverConnection;
    private float _lastSendTime;
    private Vector2 _lastSentPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        
        rb.bodyType = RigidbodyType2D.Dynamic; 

        var netManager = GameObject.FindWithTag("NetworkManager");
        if (netManager) _serverConnection = netManager.GetComponent<ServerConnection>();
    }

    void Update()
    {
        // Read Input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        if (input.magnitude > 1) input.Normalize();

        // If we are not fully connected, DO NOT try to send packets.
        if (_serverConnection == null || 
            _serverConnection.ClientConnection == null || 
            _serverConnection.ClientConnection.Client == null || 
            _serverConnection.ClientConnection.Client.User == null)
        {
            // Try to find it again if we lost it 
            if (_serverConnection == null)
            {
                var netManager = GameObject.FindWithTag("NetworkManager");
                if (netManager) _serverConnection = netManager.GetComponent<ServerConnection>();
            }
            return; // Stop here so we don't crash
        }

        // Safe to send position now
        if (Time.time - _lastSendTime > sendInterval)
        {
            if (Vector2.Distance(transform.position, _lastSentPosition) > 0.01f)
            {
                _serverConnection.ClientConnection.SendToServer(
                    new MovementPacket(
                        _serverConnection.ClientConnection.Client.User.UniqueId,
                        transform.position.x, 
                        transform.position.y
                    ));

                _lastSentPosition = transform.position;
                _lastSendTime = Time.time;
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed; 
    }
}