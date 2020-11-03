using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIAction : MonoBehaviour
{
    public GameObject optionMenu, gameMenu;


    void Start()
    {
        UIManager.Instance.OnClick += OnReplay;
        UIManager.Instance.OnOption += OnOption;
        UIManager.Instance.OnMenu += OnMenu;
    }

    public void Resume()
    {
        if (optionMenu.activeInHierarchy == true)
        {
            SoundManager.Instance.backFX.Play();
            optionMenu.SetActive(false);

            Time.timeScale = 1f; // continue scene
        }
        if (gameMenu.activeInHierarchy == true)
        {
            SoundManager.Instance.backFX.Play();
            gameMenu.SetActive(false);

            Time.timeScale = 1f; // continue scene
        }
    }

    public void OnReplay()
    {
        // Restart current game level
        SoundManager.Instance.selectFX.Play();
        SceneFader.Instance.FadeTo(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenu()
    {
        SoundManager.Instance.selectFX.Play();
        gameMenu.SetActive(true);
        Time.timeScale = 0f; // stop scene
    }

    public void OnOption()
    {
        SoundManager.Instance.selectFX.Play();
        optionMenu.SetActive(true);
        Time.timeScale = 0f; // stop scene
    }
}
