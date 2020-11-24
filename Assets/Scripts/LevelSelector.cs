using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    #region Global Variables

    [SerializeField]
    Button _levelButton;
    public bool unlocked;

    [Space(20f)]
    [SerializeField]
    Image _levelButtonImg;
    [SerializeField]
    Sprite _lvImg;
    [SerializeField]
    Sprite _starFull;
    [SerializeField]
    Image[] _stars;

    [Space(20f)]
    public string levelPageID;
    [SerializeField] GameObject _starContainer;
    //[SerializeField] GameObject _bigNumText;

    SoundManager _soundManager;
    SceneFader _sceneFader;

    #endregion


    void Start()
    {
        _soundManager = SoundManager.Instance;
        _sceneFader = SceneFader.Instance;
    }

    private void Update()
    {
        UpdateLevelStatus(); // lock or unlock level
        UpdateStarLevel(); // Collected stars number of a level
    }

    public void OnBack()
    {
        _soundManager.backFX.Play();
        _sceneFader.FadeTo(0);
    }

    private void UpdateStarLevel()
    {
        // Lock Level
        if (unlocked == false)
        {
            _levelButton.interactable = false;
        }
        // Unlock Level
        else
        {
            _levelButton.interactable = true;

            UpdateLevelButtonImg();

            // Show collected stars
            _starContainer.SetActive(true);

            // Update collected stars img
            for (int i = 0; i < PlayerPrefs.GetInt("lv" + gameObject.name); i++)
            {
                _stars[i].sprite = _starFull;
            }
        }
    }

    private void UpdateLevelButtonImg()
    {
        // Hide big number text
        //_bigNumText.SetActive(false);

        // Show level img
        _levelButtonImg.overrideSprite = _lvImg;
        _levelButtonImg.color = Color.white;
    }

    private void UpdateLevelStatus()
    {
        int prevLevelNum = int.Parse(gameObject.name) - 1;

        if (PlayerPrefs.GetInt("lv" + prevLevelNum) > 0)
        {
            unlocked = true;
        }
    }

    // Select level
    public void Select()
    {
        int levelSceneIndex = 2;
        Time.timeScale = 1f;

        PlayerPrefs.SetInt("levelID", int.Parse(gameObject.name));

        _soundManager.selectFX.Play();
        _sceneFader.FadeTo(levelSceneIndex);
    }
}
