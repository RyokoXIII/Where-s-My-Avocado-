﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    #region Singleton

    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UiManager does not exist!");

            return _instance;
        }
    }

    // Initialize instance to this class
    private void Awake()
    {
        _instance = this;
    }

    #endregion

    #region Delegate

    public event Action OnClick;
    public event Action OnPlayNext;
    public event Action OnBack;
    public event Action OnStart;
    public event Action OnMenu;
    public event Action OnSelect;
    public event Action OnOption;
    public event Action OnExit;
    public event Action OnExitMenu;
    public event Action OnBackToMainMenu;

    #endregion

    // Time delay before game over menu open
    public IEnumerator GameOverRoutine(Action onCallback = null)
    {
        yield return new WaitForSeconds(3f);

        if (onCallback != null)
            onCallback();

        Debug.Log("Begin coroutine");
    }

    // Replay Button
    public void Replay()
    {
        if (OnClick != null)
            OnClick();
    }

    public void PlayNext()
    {
        if (OnPlayNext != null)
            OnPlayNext();
    }

    // Close menu
    public void Back()
    {
        if (OnBack != null)
            OnBack();
    }

    public void BackToMainMenu()
    {
        if (OnBackToMainMenu != null)
            OnBackToMainMenu();
    }

    public void StartGame()
    {
        if (OnStart != null)
            OnStart();
    }

    // Open in-game menu 
    public void GameMenu()
    {
        if (OnMenu != null)
            OnMenu();
    }

    public void SelectLevel()
    {
        if (OnSelect != null)
            OnSelect();
    }

    // Open option menu
    public void Option()
    {
        if (OnOption != null)
            OnOption();
    }

    public void ExitGame()
    {
        if (OnExit != null)
            OnExit();
    }

    // Open exit menu
    public void ExitMenu()
    {
        if (OnExitMenu != null)
            OnExitMenu();
    }
}