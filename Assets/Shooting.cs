using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera mainCamera;
    private Vector3 mousePosition;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canShoot = true;
    private float timer;
    public float shootCooldown = 3f;
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

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
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            timer = 0f;
        }


    }
}
