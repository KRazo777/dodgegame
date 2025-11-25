using UnityEngine;
using DodgeGame.Common.Packets.Serverbound;

public class BulletScript : MonoBehaviour
{

    // Who fired bullet (Required for the Serverbound packet)
    public string OwnerId { get; set; } 
    
    // Reference to the central networking script (ASSIGIN IN INSPECTOR/CODE)
    public ServerConnection _serverConnection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float force = 3f;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        rb = GetComponent<Rigidbody2D>();

        Vector3 direction = mousePosition - transform.position;
        Vector3 rotation = transform.position - mousePosition;
 
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
        
        // send shootPacket(OwnerId, x, y)
        _serverConnection = GameObject.FindWithTag("NetworkManager").GetComponent<ServerConnection>();

        _serverConnection.ClientConnection.SendToServer(
            new ShootPacket(
                _serverConnection.ClientConnection.Client.User.UniqueId,
                transform.position.x,
                transform.position.y,
                direction.x,
                direction.y
            )
        );

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity != Vector2.zero)
        {
            // Get direction it's currently moving
            Vector2 direction = rb.linearVelocity.normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // set bullet's rotation to match
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.collider.CompareTag("Player"))
        {            
           
            var hitPlayerIdentity = collision.collider.GetComponent<ClientPlayerIdentity>(); 
            
            if (hitPlayerIdentity != null)
            {
                // Send Serverbound packet to announce hit
                if (_serverConnection != null)
                {
                    Debug.Log($"Hit detected! Sending BulletHitPacket from {OwnerId} to server.");

                    _serverConnection.ClientConnection.SendToServer(
                        new BulletHitPacket(
                            hitPlayerIdentity.UniqueId, // ID of the player who was HIT
                            OwnerId)                      // ID of the player who OWNS the bullet
                    );
                }
            }
            
            
            Destroy(gameObject);
        }
    }
}
