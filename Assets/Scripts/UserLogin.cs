using System.Collections;
using DodgeGame.Common.Packets.Serverbound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour
{
    private ServerConnection _serverConnection;
    private Button _button;
    private GameObject _textInputObject;
    private TMP_Text _inputtedText;
    AudioSource clickenter;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _serverConnection = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<ServerConnection>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        
        _textInputObject = GameObject.Find("TokenInput");
        _inputtedText = _textInputObject.transform.Find("Text Area").Find("Text").GetComponent<TMP_Text>();

        clickenter = GetComponent<AudioSource>();
    }

    private void OnClick()
    {
        clickenter.Play();
        Debug.Log("Clicked");
        Debug.Log(_inputtedText.text);
        _serverConnection.ClientConnection.SendToServer(
            new ClientAuthenticationPacket(_inputtedText.text));
    }
}
