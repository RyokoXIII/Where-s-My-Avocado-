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
    Sprite _finishedBtn;
    [SerializeField]
    Sprite _unlockedBtn, _lockedBtn;
    [SerializeField]
    GameObject[] _starList;

    [Space(20f)]
    public string levelPageID;
    [SerializeField] Image _levelBtnSprite;
    [SerializeField] GameObject _unlockedTxt, _lockedTxt;
    [SerializeField] Text _unlockedText, _lockedText;
    [SerializeField] GameObject _starContainer;

    SoundManager _soundManager;
    SceneFader _sceneFader;

    #endregion


    void Start()
    {
        _soundManager = SoundManager.Instance;
        _sceneFader = SceneFader.Instance;

        // Set Button texts
        _unlockedText.text = gameObject.name;
        _lockedText.text = gameObject.name;
    }

    private void Update()
    {
        UpdateLevelStatus();
        UpdateStarLevel();
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
            //_lockedBtn.SetActive(true);
        }
        // Unlock Level
        else
        {
            _levelButton.interactable = true;

            UpdateLevelButtonImg();

            if (_starContainer.activeInHierarchy == true)
            {
                // Update collected stars img
                for (int i = 0; i < PlayerPrefs.GetInt("lv" + gameObject.name); i++)
                {
                    if (_starList[i].activeInHierarchy == false)
                    {
                        _starList[i].SetActive(true);
                        Debug.Log("Total collected stars: " + _starList.Length.ToString());
                    }
                }
            }
        }
    }

    private void UpdateLevelButtonImg()
    {
        if (PlayerPrefs.GetInt("lv" + gameObject.name) > 0)
        {
            if (_lockedTxt.activeInHierarchy == true)
            {
                _lockedTxt.SetActive(false);
            }
            else if (_unlockedTxt.activeInHierarchy == true)
            {
                _unlockedTxt.SetActive(false);
            }

            _levelBtnSprite.overrideSprite = _finishedBtn;
            _unlockedTxt.SetActive(true);
            // Show collected stars
            _starContainer.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("lv" + gameObject.name) == 0)
        {
            //if (_lockedBtn.activeInHierarchy == true)
            //{
            //    _lockedBtn.SetActive(false);
            //}
            _levelBtnSprite.overrideSprite = _unlockedBtn;
            _unlockedTxt.SetActive(true);
        }
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
