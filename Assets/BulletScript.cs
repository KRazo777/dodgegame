using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    public float force = 3f;
    public GameObject deathSoundPrefab;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        rb = GetComponent<Rigidbody2D>();

        Vector3 direction = mousePosition - transform.position;
        Vector3 rotation = transform.position - mousePosition;
 
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
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
            Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
			Destroy(collision.collider.gameObject);
			Destroy(gameObject);
		}
    }
}
