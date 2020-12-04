using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        {
            if (_exitMenu != null)
            {
                _uiManager.ExitMenu();
            }
            else
            {
                PlayerPrefs.SetInt("backtomenu", 1);
                SceneManager.LoadScene(1);
            }
        }
    }

    void ExitMenu()
    {
        _soundManager.selectFX.Play();
        _exitMenu.SetActive(true);
    }
}
