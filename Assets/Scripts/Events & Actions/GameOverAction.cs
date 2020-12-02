using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverAction : MonoBehaviour, IAnimatable
{
    #region Global Variables

    // Image & Animation
    [Header("Animation")]
    [Space(10f)]
    [SerializeField] SkeletonGraphic _skeletonGraphic;
    [SerializeField] AnimationReferenceAsset _idle, _circle;

    string _currentAnimation;

    [Space(10f)]
    [SerializeField]
    PlayerManager _playerManager;

    [SerializeField]
    Text _stageNumText;

    [Space(10f)]
    [SerializeField] GameObject _playButton;
    [SerializeField] RectTransform _homeButton, _replayButton;
    [SerializeField] GameObject _beachMusicBackground;

    UIManager _uiManager;
    StarHandler _starHandler;
    SoundManager _soundManager;

    #endregion


    void Start()
    {
        _uiManager = UIManager.Instance;
        _starHandler = StarHandler.Instance;
        _soundManager = SoundManager.Instance;

        // Subcribe button to action event
        _uiManager.OnClick += OnReplay;
        _uiManager.OnBackToMainMenu += OnBackToMainMenu;
        _uiManager.OnPlayNext += OnPlayNext;

        // Update animation state for characters
        UpdateAnimationState();

        // Update game over state
        UpdateGameOverMenu();

        // Set Stage Number text
        _stageNumText.text = _starHandler.levelIndex.ToString();
    }

    public void OnReplay()
    {
        // Restart current game level
        _soundManager.selectFX.Play();
        SceneManager.LoadScene(1);
    }

    public void OnBackToMainMenu()
    {
        _soundManager.selectFX.Play();
        PlayerPrefs.SetInt("backtomenu", 1);

        SceneManager.LoadScene(1);
    }

    public void OnPlayNext()
    {
        _soundManager.selectFX.Play();
        PlayerPrefs.SetInt("levelID", _starHandler.levelIndex + 1);

        SceneManager.LoadScene(1);
    }

    void UpdateGameOverMenu()
    {
        Vector2 _newHomeBtnPos = new Vector2(119f, 15f);
        Vector2 _newReplayBtnPos = new Vector2(-119f, 15f);

        if (PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) > 0 && _playerManager.touchBoundary == false)
        {
            if (_starHandler.levelIndex < 51)
            {
                return;
            }
            else if (_starHandler.levelIndex == 51)
            {
                _playButton.SetActive(false);

                _homeButton.anchoredPosition = _newHomeBtnPos;
                _replayButton.anchoredPosition = _newReplayBtnPos;
            }
        }
        else
        {
            _playButton.SetActive(false);

            _homeButton.anchoredPosition = _newHomeBtnPos;
            _replayButton.anchoredPosition = _newReplayBtnPos;
        }
    }

    void UpdateAnimationState()
    {
        if (PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) > 0 && _playerManager.touchBoundary == false)
        {
            _soundManager.victoryFX.Play();

            // Win state
            SetCharacterState("idle");
        }
        else
        {
            if (_beachMusicBackground.activeInHierarchy == true)
            {
                _soundManager.beachMusicBackground.Stop();
            }
            _soundManager.loseFX.Play();

            // Lose state
            SetCharacterState("circle");
        }
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
            return;

        _skeletonGraphic.AnimationState.AddAnimation(0, animation.name, loop, 0).TimeScale = timeScale;

        _currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state == "idle")
        {
            SetAnimation(_idle, true, 1f);
        }
        else if (state == "circle")
        {
            SetAnimation(_circle, false, 1f);
        }
    }
}
