using UnityEngine;

public class LoadingSpinnerScript : MonoBehaviour
{
    
    public float rotationSpeed = 360f; 

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    // you can use this code to show the loading spinner when you start loading
    // LoadingPage.SetActive(true);

    // Once u load in disable it
    // LoadingPage.SetActive(false);
}
