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
    GameObject _starContainer;
    [SerializeField]
    Image[] _stars;
    [SerializeField]
    Sprite _starFull;

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
           
            // Show collected stars
            _starContainer.SetActive(true);

            for (int i = 0; i < PlayerPrefs.GetInt("lv" + gameObject.name); i++)
            {
                _stars[i].sprite = _starFull;
            }
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
    public void Select(int levelIndex)
    {
        Time.timeScale = 1f;

        SoundManager.Instance.selectFX.Play();
        SceneFader.Instance.FadeTo(levelIndex);
    }
}
