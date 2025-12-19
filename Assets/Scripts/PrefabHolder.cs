using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject bulletPrefab;

    // array to hold different colored bullet sprites
    // Element 0 = Red (Default)
    // Element 1 = Blue
    // Element 2 = Green
    // Element 3 = Yellow
    public Sprite[] bulletSprites; 
}