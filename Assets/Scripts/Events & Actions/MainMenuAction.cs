using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuAction : MonoBehaviour
{
    #region Global Variables

    int _scene;
    public GameObject optionMenu, exitMenu;
    public Text starNumText;

    #endregion


    void Start()
    {
        UIManager.Instance.OnBack += OnBack;
        UIManager.Instance.OnStart += OnStart;
        UIManager.Instance.OnSelect += OnSelectLevel;
        UIManager.Instance.OnOption += OnOption;
        UIManager.Instance.OnExit += OnExitGame;

        UpdateStarNumText();
    }

    void UpdateStarNumText()
    {
        int sum = 0;

        for (int i = 1; i < 20; i++)
        {
            // Add level star numbers to starNum text
            sum += PlayerPrefs.GetInt("lv" + i.ToString());
        }

        starNumText.text = sum + "/" + 60;

        Debug.Log("Star number: " + sum.ToString());
    }
    public void OnStart()
    {
        // play level 1 for first time 
        if ((PlayerPrefs.GetInt("level") + 1) == 1)
        {
            _scene = 2;
            SceneFader.Instance.FadeTo(_scene);
        }
        else
        {
            // Play next unlocked level
            int prevlevelNum = PlayerPrefs.GetInt("level") + 1;
            _scene = prevlevelNum;

            Debug.Log("Current lv: " + PlayerPrefs.GetInt("level").ToString());

            SoundManager.Instance.selectFX.Play();
            SceneFader.Instance.FadeTo(_scene);
        }
    }

    public void OnBack()
    {
        if (optionMenu.activeInHierarchy == true)
        {
            SoundManager.Instance.backFX.Play();
            optionMenu.SetActive(false);
        }
        else if (exitMenu.activeInHierarchy == true)
        {
            SoundManager.Instance.backFX.Play();
            exitMenu.SetActive(false);
        }
    }

    public void OnSelectLevel()
    {
        SoundManager.Instance.selectFX.Play();
        SceneFader.Instance.FadeTo(1);
    }

    public void OnOption()
    {
        SoundManager.Instance.selectFX.Play();
        optionMenu.SetActive(true);
    }

    public void OnExitGame()
    {
        SoundManager.Instance.selectFX.Play();
        Application.Quit();
        Debug.Log("Exit game!");
    }
}
