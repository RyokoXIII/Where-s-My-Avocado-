using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverAction : MonoBehaviour
{
    #region Global Variables

    // Image & Animation
    [SerializeField]
    Image _playerImg, _npcImg;
    [SerializeField]
    Animator _playerAnim, _npcAnim;

    [Header("Sprite")] [Space(10f)] [SerializeField]
    Sprite _playerSad;
    [SerializeField] Sprite _npcSad;

    [Space(20f)][SerializeField]
    PlayerManager _playerManager;

    [SerializeField]
    Text _stageNumText;

    [SerializeField]
    GameObject _replayMenu, _victoryMenu;
    [SerializeField]
    GameObject _sunsetMusicBackground, _beachMusicBackground;

    UIManager _uiManager;
    StarHandler _starHandler;
    SoundManager _soundManager;
    SceneFader _sceneFader;

    #endregion


    void Start()
    {
        _uiManager = UIManager.Instance;
        _starHandler = StarHandler.Instance;
        _soundManager = SoundManager.Instance;
        _sceneFader = SceneFader.Instance;

        // Subcribe button to action event
        _uiManager.OnClick += OnReplay;
        _uiManager.OnBackToMainMenu += OnBackToMainMenu;
        _uiManager.OnPlayNext += OnPlayNext;

        // Update animation state for characters
        UpdateAnimationState();

        // Set Stage Number text
        _stageNumText.text = _starHandler.levelIndex.ToString();
    }

    public void OnReplay()
    {
        gameObject.SetActive(false);

        // Restart current game level
        _soundManager.selectFX.Play();
        _sceneFader.FadeTo(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBackToMainMenu()
    {
        int scene = 0;

        _soundManager.selectFX.Play();
        _sceneFader.FadeTo(scene);
    }

    public void OnPlayNext()
    {
        if (PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) > 0 && _playerManager.touchBoundary == false)
        {
            if (_starHandler.levelIndex < 50)
            {
                _soundManager.selectFX.Play();
                _sceneFader.FadeTo(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (_starHandler.levelIndex == 50)
            {
                // Game is finished
                _soundManager.selectFX.Play();
                _victoryMenu.SetActive(true);
            }
        }
        else
        {
            _soundManager.selectFX.Play();
            _replayMenu.SetActive(true);
        }
    }

    void UpdateAnimationState()
    {
        // If win, animation play
        if (PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) > 0 && _playerManager.touchBoundary == false)
        {
            _soundManager.victoryFX.Play();

            _playerAnim.SetBool("IsWon", true);
            _npcAnim.SetBool("IsWon", true);
        }
        else
        {
            if (_sunsetMusicBackground.activeInHierarchy == true)
            {
                _soundManager.sunsetMusicBackground.Stop();
            }
            else
            {
                _soundManager.beachMusicBackground.Stop();
            }
            _soundManager.loseFX.Play();

            // Sad face
            _playerImg.overrideSprite = _playerSad;
            _npcImg.overrideSprite = _npcSad;
        }
    }
}
