using UnityEngine;
using DodgeGame.Common.Packets.Serverbound;

public class BulletScript : MonoBehaviour
{
    public string OwnerId { get; set; } 
    public ServerConnection ServerConnection;
    public int maxBounces = 3;
    private int bounceCount = 0;
    private Rigidbody2D rb;
    public float force = 10f; // Speed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ServerConnection = GameObject.FindWithTag("NetworkManager").GetComponent<ServerConnection>();

        rb.linearVelocity = transform.up * force;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;

        if (collision.collider.CompareTag("Player"))
        {            
            var hitPlayer = collision.collider.GetComponent<CharacterController>();
            
            // Don't kill the shooter, should phase thru
            if (hitPlayer != null && hitPlayer.name == OwnerId) return;

            if (ServerConnection != null)
            {
                ServerConnection.ClientConnection.SendToServer(
                    new BulletHitPacket(collision.collider.gameObject.name, OwnerId)
                );
            }
            Destroy(gameObject);
        }
        else
        {
            HandleBounce();
        }
    }
    
    private void HandleBounce()
    {
        bounceCount++;

        if (rb != null)
        {
             // Calculate angle from velocity
             float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
             
             // Apply offset (-90) because sprite points UP
             transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        if (bounceCount >= maxBounces) Destroy(gameObject);
    }
}