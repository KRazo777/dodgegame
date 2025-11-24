using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputClear : ButtonTriggerObjectVisibility
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new void Start()
    {
       base.Start();
       _button.onClick.AddListener(ClearInputs);
    }

    private void ClearInputs()
    {
        Transform roomName = parentObject.transform.Find("RoomNameInput");
        Transform password = parentObject.transform.Find("PasswordInput");
        Transform privacyToggle = parentObject.transform.Find("PrivacyToggle");
        
        if (roomName != null)
        {
            TMP_InputField input = roomName.GetComponent<TMP_InputField>();
            if (input != null) input.text = "";
        }
        
        if (password != null)
        {
            TMP_InputField input = password.GetComponent<TMP_InputField>();
            if (input != null) input.text = "";
        }

        if (privacyToggle != null)
        {
            Toggle toggle = privacyToggle.GetComponent<Toggle>();
            if (toggle != null) toggle.isOn = false;
        }
        
        Debug.Log("Input fields cleared");
    }
}
