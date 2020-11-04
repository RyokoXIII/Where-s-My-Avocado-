using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverAction : MonoBehaviour
{
    #region Global Variables

    // Image & Animation
    Image _playerImg, _npcImg;
    Animator _playerAnim, _npcAnim;
    [SerializeField]
    GameObject playerImg, npcImg;
    [SerializeField]
    Sprite playerSad, npcSad;

    PlayerManager _playerManager;
    [SerializeField]
    GameObject player;

    [SerializeField]
    Text _stageNumText;

    [SerializeField]
    GameObject _replayMenu, _victoryMenu;

    #endregion


    void Start()
    {
        // Subcribe button to action event
        UIManager.Instance.OnClick += OnReplay;
        UIManager.Instance.OnBackToMainMenu += OnBackToMainMenu;
        UIManager.Instance.OnPlayNext += OnPlayNext;

        _playerManager = player.GetComponent<PlayerManager>();

        _playerImg = playerImg.GetComponent<Image>();
        _npcImg = npcImg.GetComponent<Image>();

        _playerAnim = playerImg.GetComponent<Animator>();
        _npcAnim = npcImg.GetComponent<Animator>();

        // Update animation state for characters
        UpdateAnimationState();

        // Set Stage Number text
        _stageNumText.text = StarHandler.Instance.levelIndex.ToString();
    }

    public void OnReplay()
    {
        gameObject.SetActive(false);

        // Restart current game level
        SoundManager.Instance.selectFX.Play();
        SceneFader.Instance.FadeTo(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBackToMainMenu()
    {
        SoundManager.Instance.selectFX.Play();
        SceneFader.Instance.FadeTo("Main_Menu");
    }

    public void OnPlayNext()
    {
        if (PlayerPrefs.GetInt("lv" + StarHandler.Instance.levelIndex) > 0 && _playerManager.touchBoundary == false)
        {
            if (StarHandler.Instance.levelIndex < 50)
            {
                SoundManager.Instance.selectFX.Play();
                SceneFader.Instance.FadeTo(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (StarHandler.Instance.levelIndex == 50)
            {
                // Game is finished
                SoundManager.Instance.selectFX.Play();
                _victoryMenu.SetActive(true);
            }
        }
        else
        {
            SoundManager.Instance.selectFX.Play();
            _replayMenu.SetActive(true);
        }
    }

    void UpdateAnimationState()
    {
        // If win, animation play
        if (PlayerPrefs.GetInt("lv" + StarHandler.Instance.levelIndex) > 0 && _playerManager.touchBoundary == false)
        {
            SoundManager.Instance.victoryFX.Play();
            _playerAnim.SetBool("IsWon", true);
            _npcAnim.SetBool("IsWon", true);
        }
        else
        {
            // Sad face
            SoundManager.Instance.musicBackground.Stop();
            SoundManager.Instance.loseFX.Play();
            _playerImg.overrideSprite = playerSad;
            _npcImg.overrideSprite = npcSad;
        }
    }
}
