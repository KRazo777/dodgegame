using DodgeGame.Common.Packets.Serverbound;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera mainCamera;
    private Vector3 mousePosition;
    private ServerConnection _serverConnection;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canShoot = true;
    private float timer;
    public float shootCooldown = 3f;
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _serverConnection = GameObject.FindWithTag("NetworkManager").GetComponent<ServerConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handles the direction of the bullet BEFORE FIRING based on the mouse position
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePosition - transform.position;

        float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
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

        // Handles the shooting of the bullet
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && canShoot)
        {
            canShoot = false;
            _serverConnection.ClientConnection.SendToServer(
                new BulletFiredPacket(
                    _serverConnection.ClientConnection.Client.User.UniqueId,
                    bulletTransform.position.x,
                    bulletTransform.position.y,
                    transform.rotation.x,
                    transform.rotation.y,
                    transform.rotation.z,
                    transform.rotation.w));
            timer = 0f;
        }


    }
}
