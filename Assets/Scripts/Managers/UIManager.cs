using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoSingleton<UIManager>
{
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
    public event Action OnUpgrade;
    public event Action OnGetCoin;
    public event Action OnGetArmor;
    public event Action OnLooseIt;
    public event Action OnContinue;
    public event Action OnGetTriple;
    public event Action OnNextToPowerUp;

    #endregion

    // Time delay before game over menu open
    public IEnumerator GameOverRoutine(Action onCallback = null)
    {
        yield return new WaitForSeconds(3f);

        if (onCallback != null)
            onCallback();
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

    public void Upgrade()
    {
        if (OnUpgrade != null)
            OnUpgrade();
    }

    public void GetCoin()
    {
        if (OnGetCoin != null)
            OnGetCoin();
    }

    public void GetArmor()
    {
        if (OnGetArmor != null)
            OnGetArmor();
    }

    public void LooseIt()
    {
        if (OnLooseIt != null)
            OnLooseIt();
    }

    public void Continue()
    {
        if (OnContinue != null)
            OnContinue();
    }

    public void TripleCoin()
    {
        if (OnGetTriple != null)
            OnGetTriple();
    }

    public void NextToPowerUp()
    {
        if (OnNextToPowerUp != null)
            OnNextToPowerUp();
    }
}
