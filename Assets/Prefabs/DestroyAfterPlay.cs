using UnityEngine;

public class DestroyAfterPlay : MonoBehaviour
{
    void Start()
    {
        AudioSource die = GetComponent<AudioSource>();

        die.Play();

        Destroy(gameObject, die.clip.length);
    }
}