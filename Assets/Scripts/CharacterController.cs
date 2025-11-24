using System;
using DodgeGame.Common.Packets.Serverbound;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
	[SerializeField] bool isActive = true;
	[SerializeField] float moveSpeed = 5f;
	
    Rigidbody2D rb;
    Vector2 input;

    private ServerConnection _serverConnection;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _serverConnection = GetComponent<ServerConnection>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // WASD input
		if (isActive) {
            input.x = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1 : 0) -
                      (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
            input.y = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1 : 0) -
                      (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
            input = Vector2.ClampMagnitude(input, 1f); // normalize diagonals
		}
    }

    void FixedUpdate()
    {
        var old = rb.linearVelocity;
        rb.linearVelocity = input * moveSpeed;
        if (rb.linearVelocity != old)
        {
            _serverConnection.ClientConnection.SendToServer(
                new MovementPacket(
                    _serverConnection.UniqueId,
                    transform.position.x,
                    transform.position.y));
        }
    }
}