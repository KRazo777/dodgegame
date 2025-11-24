using UnityEngine;
using UnityEngine.UI;

public class CreateToggle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject objectToToggle;
    public bool toggleTo = true;
    
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ToggleTarget);
        }
    }

    private void ToggleTarget()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(toggleTo);
        }
    }
}
