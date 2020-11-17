using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToExit : MonoBehaviour
{
    [SerializeField]
    GameObject _exitMenu;

    UIManager _uiManager;
    SoundManager _soundManager;


    private void Start()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

        _uiManager.OnExitMenu += ExitMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _uiManager.ExitMenu();
    }

    void ExitMenu()
    {
        _soundManager.selectFX.Play();
        _exitMenu.SetActive(true);
    }
}
