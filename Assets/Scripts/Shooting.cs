using DodgeGame.Common.Packets.Serverbound;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePosition;
    private ServerConnection _serverConnection;
    
    public GameObject bullet; 
    public Transform bulletTransform;
    
    // We will find this automatically in code
    private Collider2D playerCollider;
    
    public bool canShoot = true;
    private float timer;
    public float shootCooldown = 3f;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _serverConnection = GameObject.FindWithTag("NetworkManager").GetComponent<ServerConnection>();
        
        playerCollider = GetComponentInParent<Collider2D>();
    }

    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;

        // Calculate Angle
        float rotz = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, rotz);

        if (!canShoot)
        {
            timer += Time.deltaTime;
            if (timer >= shootCooldown)
            {
                canShoot = true;
                timer = 0;
            }
        }

        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && canShoot)
        {
            canShoot = false;
            string myId = _serverConnection.ClientConnection.Client.User.UniqueId;

            // SPAWN BULLET (Keep -90 offset ONLY for the bullet sprite)
            GameObject localBullet = Instantiate(bullet, bulletTransform.position, Quaternion.Euler(0, 0, rotz - 90));
            localBullet.GetComponent<BulletScript>().OwnerId = myId;

            // This forces the physics engine to ignore collisions between YOU and YOUR BULLET.
            Collider2D bulletCollider = localBullet.GetComponent<Collider2D>();
            
            if (playerCollider != null && bulletCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, bulletCollider);
            }

            if (_serverConnection != null)
            {
                _serverConnection.ClientConnection.SendToServer(
                    new BulletFiredPacket(myId, bulletTransform.position.x, bulletTransform.position.y, rotz)
                );
            }
            timer = 0f;
        }
    }
}