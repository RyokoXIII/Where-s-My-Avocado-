using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    #region Global Variables

    Button levelButton;
    public bool unlocked;

    [SerializeField]
    Sprite _lvImg;
    [SerializeField]
    Sprite _starFull;
    [SerializeField]
    Image[] _stars;

    [Space(20f)] [SerializeField]
    GameObject _starContainer;
    [SerializeField]
    GameObject _tinyNumText, _bigNumText;

    #endregion


    void Start()
    {
        levelButton = GetComponent<Button>();
        UIManager.Instance.OnBack += OnBack;

        //PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        UpdateLevelStatus(); // lock or unlock level
        UpdateStarLevel(); // Collected stars number of a level
    }

    public void OnBack()
    {
        SoundManager.Instance.backFX.Play();
        SceneFader.Instance.FadeTo(0);
    }

    private void UpdateStarLevel()
    {
        // Lock Level
        if (unlocked == false)
        {
            levelButton.interactable = false;
        }
        // Unlock Level
        else
        {
            levelButton.interactable = true;

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
        _bigNumText.SetActive(false);

        // Show level img
        gameObject.GetComponent<Image>().overrideSprite = _lvImg;
        gameObject.GetComponent<Image>().color = Color.white;

        // Show tiny level number text
        _tinyNumText.GetComponent<Text>().text = gameObject.name;
        _tinyNumText.SetActive(true);
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
    public void Select(int levelIndex)
    {
        Time.timeScale = 1f;

        SoundManager.Instance.selectFX.Play();
        SceneFader.Instance.FadeTo(levelIndex);
    }
}
