using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ButtonTriggerObjectVisibility : MonoBehaviour
{
    private ServerConnection _serverConnection;
    
    public GameObject objectToToggle;
    public bool toggleTo;

    public GameObject parentObject;
    public bool toggleParentTo;

    public bool requiresAuth;
    
    public Button _button;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        _serverConnection = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<ServerConnection>();
        
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SwitchScene);
    }

    private void SwitchScene()
    {
        if (requiresAuth && _serverConnection.ClientConnection.Client.User == null)
        {
            return;
        }
        objectToToggle!.SetActive(toggleTo);
        parentObject?.transform.gameObject.SetActive(toggleParentTo);
    }
}
