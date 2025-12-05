using UnityEngine;
using DodgeGame.Common.Packets.Serverbound;

public class BulletScript : MonoBehaviour
{

    // Who fired bullet (Required for the Serverbound packet)
    public string OwnerId { get; set; } 
    
    // Reference to the central networking script (ASSIGIN IN INSPECTOR/CODE)
    public ServerConnection ServerConnection;

    public int maxBounces = 3;

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
        
        ServerConnection = GameObject.FindWithTag("NetworkManager").GetComponent<ServerConnection>();
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.collider.CompareTag("Player"))
        {            
           
            // Send Serverbound packet to announce hit
            if (ServerConnection != null)
            {
                Debug.Log($"Hit {collision.collider.gameObject.name} detected! Sending BulletHitPacket from {OwnerId} to server.");

                ServerConnection.ClientConnection.SendToServer(
                    new BulletHitPacket(
                        collision.collider.gameObject.name, // ID of the player who was HIT
                        OwnerId)                      // ID of the player who OWNS the bullet
                );
            }
            
            
            Destroy(gameObject);
        }
    }
}
