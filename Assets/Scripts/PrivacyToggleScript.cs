using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrivacyToggleScript : MonoBehaviour
{
    public TMP_InputField passwordInputField;
    public Toggle privateToggle; 

    void Start()
    {
        privateToggle.onValueChanged.AddListener(OnToggleChanged);

        OnToggleChanged(privateToggle.isOn);
    }

    private void OnToggleChanged(bool isChecked)
    {
        passwordInputField.interactable = isChecked;

        
        //clear pw if the user switches back to public
        if (!isChecked)
        {
            passwordInputField.text = "";
        }
    }
}
