using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ButtonTriggerObjectVisibility : MonoBehaviour
{
    public GameObject objectToToggle;
    public bool toggleTo;

    public GameObject parentObject;
    public bool toggleParentTo;
    
    private Button _button;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SwitchScene);
    }

    private void SwitchScene()
    {
        objectToToggle!.SetActive(toggleTo);
        parentObject?.transform.gameObject.SetActive(toggleParentTo);
    }
}
