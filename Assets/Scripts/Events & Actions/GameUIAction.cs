using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIAction : MonoBehaviour
{
    #region Global Variables

    public GameObject optionMenu, gameMenu;

    UIManager _uiManager;
    SoundManager _soundManager;
    SceneFader _sceneFader;

    #endregion


    void Start()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;
        _sceneFader = SceneFader.Instance;

        _uiManager.OnClick += OnReplay;
        _uiManager.OnOption += OnOption;
        _uiManager.OnMenu += OnMenu;
    }

    public void Resume()
    {
        if (optionMenu.activeInHierarchy == true)
        {
            _soundManager.backFX.Play();
            optionMenu.SetActive(false);

            Time.timeScale = 1f; // continue scene
        }
        if (gameMenu.activeInHierarchy == true)
        {
            _soundManager.backFX.Play();
            gameMenu.SetActive(false);

            Time.timeScale = 1f; // continue scene
        }
    }

    public void OnReplay()
    {
        // Restart current game level
        _soundManager.selectFX.Play();
        _sceneFader.FadeTo(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenu()
    {
        _soundManager.selectFX.Play();
        gameMenu.SetActive(true);
        Time.timeScale = 0f; // stop scene
    }

    public void OnOption()
    {
        _soundManager.selectFX.Play();
        optionMenu.SetActive(true);
        Time.timeScale = 0f; // stop scene
    }
}
