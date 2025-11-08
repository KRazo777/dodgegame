using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DodgeGame.Common.Game;
using Object = UnityEngine.Object;

public class ButtonSceneChanger : MonoBehaviour
{
    [SerializeField]
    public Object sceneToChange;
    private Button _button;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SwitchScene);
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene(sceneToChange.name);
    }
}
