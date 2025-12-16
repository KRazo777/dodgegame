using UnityEngine;

public class NetworkInterpolator : MonoBehaviour
{
    private Vector3 _targetPosition;
    
    // 20.0f is a good snap speed 
    // Higher = Snappier but jittery, lower = smoother but floatier
    [SerializeField] private float smoothSpeed = 15.0f; 

    void Awake()
    {
        // Start where we are so we don't fly in from (0,0,0)
        _targetPosition = transform.position;
    }

    public void UpdateTargetPosition(Vector2 pos)
    {
        _targetPosition = new Vector3(pos.x, pos.y, transform.position.z);
    }

    void Update()
    {
        // Smoothly move 90% of the way to the target every frame
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * smoothSpeed);
    }
}