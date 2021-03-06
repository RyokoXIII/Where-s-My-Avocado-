﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuAction : MonoBehaviour
{
    #region Global Variables

    public GameObject optionMenu, exitMenu, selectMenu;
    public Text starNumText;

    int _maxLevels = 100;

    UIManager _uiManager;
    SoundManager _soundManager;

    #endregion


    void Start()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

        _uiManager.OnBack += OnBack;
        _uiManager.OnSelect += OnSelectLevel;
        _uiManager.OnOption += OnOption;
        _uiManager.OnExit += OnExitGame;

        UpdateStarNumText();
    }

    void UpdateStarNumText()
    {
        int sum = 0;

        for (int i = 1; i <= 100; i++)
        {
            // Add level star numbers to starNum text
            sum += PlayerPrefs.GetInt("lv" + i.ToString());
        }

        starNumText.text = sum.ToString();

        Debug.Log("Star number: " + sum.ToString());
    }

    public void OnBack()
    {
        if (optionMenu.activeInHierarchy == true)
        {
            _soundManager.backFX.Play();
            optionMenu.SetActive(false);
        }
        else if (exitMenu.activeInHierarchy == true)
        {
            _soundManager.backFX.Play();
            exitMenu.SetActive(false);
        }
        else if (selectMenu.activeInHierarchy == true)
        {
            _soundManager.backFX.Play();
            selectMenu.SetActive(false);
        }
    }

    public void OnSelectLevel()
    {
        _soundManager.selectFX.Play();

        SceneManager.LoadScene(1);
    }

    public void OnOption()
    {
        _soundManager.selectFX.Play();
        optionMenu.SetActive(true);
    }

    public void OnExitGame()
    {
        _soundManager.selectFX.Play();
        Application.Quit();
        Debug.Log("Exit game!");
    }
}
