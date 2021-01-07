using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerManager : MonoBehaviour, IAnimatable, IDamageable
{
    #region Global Variables

    // Manager
    PoolManager _pooler;
    UIManager _uiManager;
    StarHandler _starHandler;
    SoundManager _soundManager;

    Vector2 _force;
    public float magnitude = 5f;

    [Space(10f)]
    [SerializeField]
    Rigidbody2D _playerRb;
    [SerializeField]
    CircleCollider2D _playerColl;
    [SerializeField]
    Transform _bossPos;
    [SerializeField]
    GameObject gameOverContainer;
    [SerializeField]
    BossManager _bossManager;

    // Animation
    [Space(20f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _circle, _rollout, _finished1, _finished2, _finished3, _attack;
    [SerializeField] int _playerState;

    string _currentAnimation;

    int _enemyCount = 0;
    int _cantCollideLayerIndex;

    [HideInInspector]
    public int finalScore;

    [HideInInspector]
    public bool touchBoundary, hasFirstStar;
    public bool touchBoss;

    Vector3 _scaleChangeRight, _scaleChangeLeft;
    bool checkFlipPlayer;
    bool bePushed, splash;

    [Header("Player Stats")]
    [Space(10f)]
    [SerializeField] LevelUpSystem _levelSystem;
    public int playerDamage, takeDamagePoint;
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBarscript;
    public GameObject healthBar;

    private float t = 0.0f;
    private float threshold = 1f;

    bool _gameOver, _statsUpdate;

    #endregion

    void Start()
    {
        _pooler = PoolManager.Instance;
        _uiManager = UIManager.Instance;
        _starHandler = StarHandler.Instance;
        _soundManager = SoundManager.Instance;

        _cantCollideLayerIndex = LayerMask.NameToLayer("End");

        // Start Animation callback
        StartCoroutine(StartAnimationTransition());

        _scaleChangeRight = new Vector3(0.5f, 0.5f, 0.5f);
        _scaleChangeLeft = new Vector3(-0.5f, 0.5f, 0.5f);
        checkFlipPlayer = true;

        // Random player finished anim
        _playerState = Random.Range(0, 3);

        // Set player stats
        SetHealthStats();
        playerDamage = _levelSystem.attack;
    }

    void SetTakeDamagePoint()
    {
        if (_statsUpdate == false)
        {
            takeDamagePoint = _bossManager.bossDamage;
            _statsUpdate = true;
        }
    }

    void SetHealthStats()
    {
        maxHealth = _levelSystem.maxHP;
        currentHealth = maxHealth;
        healthBarscript.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        t += Time.deltaTime;

        if (t >= threshold)
        {
            t = 0.0f;

            if (currentHealth > damage)
            {
                currentHealth -= damage;
                healthBarscript.SetCurrentHealth(currentHealth);
            }
            else if (damage > currentHealth)
            {
                currentHealth = 0;
                healthBarscript.SetCurrentHealth(currentHealth);
            }
        }
    }

    private void Update()
    {
        SetTakeDamagePoint();

        FlipPlayerSprite();

        if (touchBoss == true && _bossManager.currentHealth != 0)
        {
            TakeDamage(takeDamagePoint);
        }
        else if (_bossManager.currentHealth == 0 && _gameOver == false)
        {
            _gameOver = true;
            StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
        }
    }

    void FlipPlayerSprite()
    {
        if (checkFlipPlayer && (Vector2.Distance(transform.position, _bossPos.position) < 10f))
        {
            if (_bossPos.transform.localScale.x > 0)
            {
                checkFlipPlayer = false;
                transform.localScale = _scaleChangeRight;
            }
            else
            {
                checkFlipPlayer = false;
                transform.localScale = _scaleChangeLeft;
            }
        }

        if (touchBoss == false)
        {
            if ((Vector2.Distance(transform.position, _bossPos.position) < 3f) && transform.position.y < _bossPos.position.y + 2f)
            {
                if (transform.position.x < _bossPos.position.x)
                {
                    transform.localScale = _scaleChangeRight;
                }
                else
                {
                    transform.localScale = _scaleChangeLeft;
                }

                touchBoss = true;
                gameObject.layer = _cantCollideLayerIndex; // change layer

                healthBar.SetActive(true);

                // Rotate smoothly to 0
                StartCoroutine(AnimateRotationTowards(this.transform, Quaternion.identity, .1f));
                StartCoroutine(WinAnimationTransition()); // Kill boss animation
                //StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
            }
        }
    }

    // Check trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.enabled == true)
            {
                _enemyCount += 1;
                finalScore = _enemyCount;

                hasFirstStar = true;
                Debug.Log("Enemy killed: " + _enemyCount.ToString());

                // Disabled trigger event
                other.enabled = false;
            }
        }

        if (other.gameObject.CompareTag("Water"))
        {
            if (splash == false)
            {
                // Particle
                Vector2 Splashpos = new Vector2(transform.position.x, transform.position.y - 0.2f);
                _pooler.SpawnFromPool("WaterSplash Particle", Splashpos, Quaternion.Euler(-90, 0, 0));

                _soundManager.waterSplashFX.Play();

                // Game Over menu pop up Action callback
                StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
                splash = true;
                touchBoundary = true;
            }
        }
    }

    IEnumerator AnimateRotationTowards(Transform target, Quaternion rot, float dur)
    {
        float time = 0f;
        Quaternion start = target.rotation;
        while (time < dur)
        {
            target.rotation = Quaternion.Slerp(start, rot, time / dur);
            yield return null;
            time += Time.deltaTime;
        }
        target.rotation = rot;

        _playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerRb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    IEnumerator WinAnimationTransition()
    {
        yield return new WaitForSeconds(0);

        if (_playerState == 0)
        {
            SetCharacterState("finisher");
        }
        else if (_playerState == 1)
        {
            SetCharacterState("finisher2");
        }
        else
        {
            SetCharacterState("finisher3");
        }

        while (_bossManager.currentHealth > 0)
        {

            yield return new WaitForSeconds(0.5f);
            SetCharacterState("atk");

        }

        yield return new WaitForSeconds(1f);
        SetCharacterState("idle");
        yield return new WaitForSeconds(1f);
        healthBar.SetActive(false);
    }

    IEnumerator StartAnimationTransition()
    {
        SetCharacterState("idle");

        yield return new WaitForSeconds(0.5f);

        SetCharacterState("circle");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Push_line") && (bePushed == false))
        {
            // calculate force vector
            _force = transform.position - other.transform.position;
            // normalize force vector to get direction only and trim magnitude
            _force.Normalize();
            _playerRb.AddForce(_force.normalized * magnitude, ForceMode2D.Impulse);

            bePushed = true;
        }

        if (other.gameObject.CompareTag("Boundary"))
        {
            // Particle
            _pooler.SpawnFromPool("BoundaryParticle", transform.position, Quaternion.identity);

            gameObject.transform.rotation = Quaternion.identity;
            _playerRb.constraints = RigidbodyConstraints2D.FreezeAll;

            touchBoundary = true;

            // Game Over menu pop up Action callback
            StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
        }
    }

    // Set player animation
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
            return;

        _skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        _currentAnimation = animation.name;
    }

    // Check player state and set animation accordingly
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
        else if (state == "roll out")
        {
            SetAnimation(_rollout, false, 1.5f);
        }
        else if (state == "finisher")
        {
            SetAnimation(_finished1, false, 1.5f);
        }
        else if (state == "finisher2")
        {
            SetAnimation(_finished2, false, 1.5f);
        }
        else if (state == "finisher3")
        {
            SetAnimation(_finished3, false, 1.5f);
        }
        else if (state == "atk")
        {
            SetAnimation(_attack, true, 1f);
        }
    }

    // Game over menu popup
    void GameOverPopup()
    {
        gameOverContainer.SetActive(true);

        SaveCollectedStarsNum();
    }

    void SaveCollectedStarsNum()
    {
        // Kill boss without killing enemies
        if (touchBoss == true && _enemyCount == 0)
        {
            finalScore = 1;
        }

        if (touchBoundary == false)
        {
            StarHandler.Instance.StarAchieved(finalScore);
            SaveNextLevel();

            // Save score if > previous score
            if (finalScore > PlayerPrefs.GetInt("lv" + _starHandler.levelIndex))
            {
                PlayerPrefs.SetInt("lv" + _starHandler.levelIndex, finalScore);
            }
        }
        Debug.Log("Collected Stars: " + PlayerPrefs.GetInt("lv" + _starHandler.levelIndex));
    }

    // Save newest unlocked level
    void SaveNextLevel()
    {
        if (PlayerPrefs.GetInt("level") <= _starHandler.levelIndex)
        {
            if (_starHandler.levelIndex < 100)
            {
                PlayerPrefs.SetInt("level", _starHandler.levelIndex + 1);
            }
        }
        Debug.Log("Next level: " + PlayerPrefs.GetInt("level"));
    }
}
