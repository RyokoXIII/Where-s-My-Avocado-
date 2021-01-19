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
    [SerializeField] GameObject _heroPos;
    [SerializeField] AnimationReferenceAsset _idle, _circle;

    [Space(10f)]
    [SerializeField] SkeletonDataAsset _newDataAsset;
    [SerializeField] AnimationReferenceAsset _idle2, _circle2;

    string _currentAnimation;

    [Space(10f)]
    [SerializeField]
    PlayerManager _playerManager;
    [SerializeField] LevelUpSystem _playerLvUp;
    [SerializeField] Text _damageTxt, _healthTxt, _expPointTxt, _nextExpPointTxt;
    [SerializeField] GameObject _damagePlus, _healthPlus;
    [SerializeField] Animator _strengthAnim, _healthAnim;
    Text _damagePlusTxt, _healthPlusTxt;

    bool _hasNotUpgrade;
    int _maxLevels = 100;

    [Space(10f)]
    [SerializeField]
    Text _stageNumText;
    [SerializeField] GameObject _menu, _menuUpgrade;
    [SerializeField] GameObject _playButton;
    [SerializeField] RectTransform _homeButton, _replayButton;
    [SerializeField] GameObject _beachMusicBackground;

    UIManager _uiManager;
    StarHandler _starHandler;
    SoundManager _soundManager;
    PoolManager _pooler;

    #endregion

    void Start()
    {
        _uiManager = UIManager.Instance;
        _starHandler = StarHandler.Instance;
        _soundManager = SoundManager.Instance;
        _pooler = PoolManager.Instance;

        _damagePlusTxt = _damagePlus.GetComponent<Text>();
        _healthPlusTxt = _healthPlus.GetComponent<Text>();

        // Subcribe button to action event
        _uiManager.OnClick += OnReplay;
        _uiManager.OnBackToMainMenu += OnBackToMainMenu;
        _uiManager.OnPlayNext += OnPlayNext;
        _uiManager.OnUpgrade += OnUpgrade;
        _uiManager.OnSkip += OnSkip;

        if (PlayerPrefs.GetInt("level") == 21)
        {
            Vector3 pos = new Vector3(_heroPos.transform.position.x, _heroPos.transform.position.y + 1, 1f);
            _pooler.SpawnFromPool("Upgrade Particle", pos, Quaternion.identity);
            SetAnimationUpgrade();
        }
        else if (PlayerPrefs.GetString("upgradeHero") == "upgraded")
        {
            SetAnimationUpgrade();
        }

        // Update animation state for characters
        UpdateAnimationState();

        // Update game over state
        UpdateGameOverMenu();

        // Set Stage Number text
        _stageNumText.text = _starHandler.levelIndex.ToString();

        _hasNotUpgrade = true;

        GetPlayerCurrentStats();

        // Save exp point
        if (_playerManager.touchBoundary == false)
        {
            PlayerPrefs.SetInt("expPoint", _playerLvUp.currentExp);
        }
    }

    private void Update()
    {
        UpgradePlayerStats();
        UpdateExpPoint();

        _nextExpPointTxt.text = "/ " + _playerLvUp.nextLevelExp.ToString() + " pts";

        if (_playerLvUp.currentExp < _playerLvUp.nextLevelExp)
        {
            // Deactivated point plus
            _damagePlus.SetActive(false);
            _healthPlus.SetActive(false);
        }
        else if (_playerLvUp.characterLevel == _playerLvUp.characterMaxLevel)
        {
            // Deactivated point plus
            _damagePlus.SetActive(false);
            _healthPlus.SetActive(false);
        }
    }

    void GetPlayerCurrentStats()
    {
        _damageTxt.text = _playerLvUp.attack.ToString();
        _healthTxt.text = _playerLvUp.maxHP.ToString();

        // Stats upgrade point
        _damagePlusTxt.text = "+ 50";
        _healthPlusTxt.text = "+ 250";
    }

    void UpgradePlayerStats()
    {
        if (_hasNotUpgrade == false)
        {
            if (_playerLvUp.currentExp >= _playerLvUp.nextLevelExp &&
            _playerLvUp.characterLevel < _playerLvUp.characterMaxLevel)
            {
                Vector3 pos = new Vector3(_heroPos.transform.position.x, _heroPos.transform.position.y + 0.8f, 1f);
                _pooler.SpawnFromPool("levelup Particle", pos, Quaternion.identity);

                // Change text color
                _damageTxt.color = new Color(0, 0.6509434f, 0.08934521f, 1f);
                _healthTxt.color = new Color(0, 0.6509434f, 0.08934521f, 1f);

                _playerLvUp.AddExp();

                PlayerPrefs.SetInt("expPoint", _playerLvUp.currentExp);

                // Update player stats
                _damageTxt.text = _playerLvUp.attack.ToString();
                _healthTxt.text = _playerLvUp.maxHP.ToString();

                PlayerPrefs.SetInt("damageStats", _playerLvUp.attack);
                PlayerPrefs.SetInt("healthStats", _playerLvUp.maxHP);
                PlayerPrefs.SetInt("playerLv", _playerLvUp.characterLevel);

                _hasNotUpgrade = true;

                _strengthAnim.Play("Transform_strength");
                _healthAnim.Play("Transform_health");
            }
        }
    }

    void UpdateExpPoint()
    {
        if (_playerLvUp.currentExp >= _playerLvUp.nextLevelExp)
        {
            _expPointTxt.color = Color.green;
        }
        else
        {
            _expPointTxt.color = Color.red;
        }

        _expPointTxt.text = _playerLvUp.currentExp.ToString();
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

        if (_starHandler.levelIndex == _maxLevels)
        {
            PlayerPrefs.SetInt("levelID", 1);
        }
        else
        {
            PlayerPrefs.SetInt("levelID", _starHandler.levelIndex + 1);
        }

        SceneManager.LoadScene(1);
    }

    public void OnUpgrade()
    {
        _hasNotUpgrade = false;
    }

    public void OnSkip()
    {
        _menuUpgrade.SetActive(false);
        _menu.SetActive(true);
    }

    void UpdateGameOverMenu()
    {
        Vector2 _newHomeBtnPos = new Vector2(-100f, 15f);
        Vector2 _newReplayBtnPos = new Vector2(100f, 15f);

        if (_playerLvUp.currentExp < _playerLvUp.nextLevelExp)
        {
            _menuUpgrade.SetActive(false);
            _menu.SetActive(true);
        }

        if (PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) > 0 && _playerManager.touchBoundary == false)
        {
            return;
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
        if (PlayerPrefs.GetString("upgradeHero") == "upgraded")
        {
            if (state == "idle")
            {
                SetAnimation(_idle2, true, 1f);
            }
            else if (state == "circle")
            {
                SetAnimation(_circle2, false, 1f);
            }
        }
        else
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

    public void SetAnimationUpgrade()
    {
        _skeletonGraphic.Clear();
        _skeletonGraphic.skeletonDataAsset = _newDataAsset;
        _skeletonGraphic.Initialize(true);
    }
}
