using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerManager : MonoBehaviour, IAnimatable
{
    #region Global Variables

    PoolManager _pooler;
    UIManager _uiManager;
    StarHandler _starHandler;

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

    [Space(20f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _circle, _rollout, _finished;
    [SerializeField] string _currentState;

    string _currentAnimation;

    int _starCount = 0;
    int _cantCollideLayerIndex;

    [HideInInspector]
    public int finalScore;

    [HideInInspector]
    public bool touchBoundary, hasFirstStar, hasKillBoss;

    public bool touchGround;

    Vector3 _scaleChangeRight, _scaleChangeLeft;
    bool checkFlipPlayer;
    bool bePushed;

    #endregion


    void Start()
    {
        _pooler = PoolManager.Instance;
        _uiManager = UIManager.Instance;
        _starHandler = StarHandler.Instance;

        _cantCollideLayerIndex = LayerMask.NameToLayer("End");

        // Start Animation callback
        StartCoroutine(StartAnimationTransition());

        _scaleChangeRight = new Vector3(0.5f, 0.5f, 0.5f);
        _scaleChangeLeft = new Vector3(-0.5f, 0.5f, 0.5f);
        checkFlipPlayer = true;
    }

    private void Update()
    {
        FlipPlayerSprite();
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

        if (touchGround == false)
        {
            //Debug.Log(Vector2.Distance(transform.position, _bossPos.position).ToString());
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

                touchGround = true;
                gameObject.layer = _cantCollideLayerIndex; // change layer

                // Rotate smoothly to 0
                StartCoroutine(AnimateRotationTowards(this.transform, Quaternion.identity, .1f));
                StartCoroutine(WinAnimationTransition()); // Kill boss animation

                // Game Over menu pop up Action callback
                StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _starCount += 1;
            finalScore = _starCount;
            Debug.Log("Enemy killed: " + _starCount.ToString());

            hasFirstStar = true;
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
        yield return new WaitForSeconds(0f);

        SetCharacterState("finisher");
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
            CreateParticleEffect();

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
            SetAnimation(_finished, false, 1.5f);
        }
    }

    void CreateParticleEffect()
    {
        _pooler.SpawnFromPool("BoundaryParticle", transform.position, Quaternion.identity);
    }

    // Game over menu popup
    void GameOverPopup()
    {
        gameOverContainer.SetActive(true);

        SaveCollectedStarsNum();
    }

    void SaveCollectedStarsNum()
    {
        if (finalScore > 3)
        {
            finalScore = 3;
        }
        StarHandler.Instance.StarAchieved(finalScore);

        if (finalScore > PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) && touchBoundary == false)
        {
            PlayerPrefs.SetInt("lv" + _starHandler.levelIndex, finalScore);
            SaveNextLevel();
        }

        Debug.Log("Collected Stars: " + PlayerPrefs.GetInt("lv" + _starHandler.levelIndex));
    }

    // Save newest unlocked level
    void SaveNextLevel()
    {
        if (PlayerPrefs.GetInt("level") <= _starHandler.levelIndex)
        {
            if (_starHandler.levelIndex < 55)
            {
                PlayerPrefs.SetInt("level", _starHandler.levelIndex + 1);
            }
        }
        Debug.Log("Next level: " + PlayerPrefs.GetInt("level"));
    }
}
