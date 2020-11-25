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
    Transform _bossPos;
    [SerializeField]
    GameObject gameOverContainer;
    [SerializeField]
    BossManager _bossManager;

    [Space(20f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _circle, _rollout;
    [SerializeField] string _currentState;

    float _currentPos;
    string _currentAnimation;

    int _starCount = 0;
    [HideInInspector]
    public int finalScore;

    [HideInInspector]
    public bool touchBoundary, hasFirstStar, touchBoss;

    #endregion


    void Start()
    {
        _pooler = PoolManager.Instance;
        _uiManager = UIManager.Instance;
        _starHandler = StarHandler.Instance;

        _currentPos = transform.position.x;

        StartCoroutine(StartAnimationTransition());
    }

    private void Update()
    {
        if ((_currentPos > transform.position.x) || (_currentPos < transform.position.x))
        {
            if (touchBoss == false)
            {
                SetCharacterState("circle");
            }
        }
        _currentPos = transform.position.x;
    }

    //IEnumerator CheckPlayerMovement()
    //{
    //    Vector3 startPos = gameObject.transform.position;
    //    yield return new WaitForSeconds(1f);

    //    Vector3 finalPos = gameObject.transform.position;

    //    if (startPos.x != finalPos.x || startPos.y != finalPos.y
    //        || startPos.z != finalPos.z)
    //    {
    //        _playerIsMoving = true;
    //        SetPlayerState("circle");
    //    }
    //    else
    //    {
    //        _playerIsMoving = false;
    //        SetPlayerState("idle");
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _starCount += 1;
            finalScore = _starCount;

            hasFirstStar = true;
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            gameObject.transform.rotation = Quaternion.identity;
            Vector2 fixPos = new Vector2(_bossPos.position.x, _bossPos.position.y);
            transform.position = fixPos;
            _playerRb.constraints = RigidbodyConstraints2D.FreezeAll;

            touchBoss = true;
            StartCoroutine(WinAnimationTransition());

            // Game Over menu pop up Action callback
            StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
        }
    }

    IEnumerator WinAnimationTransition()
    {
        SetCharacterState("roll out");

        yield return new WaitForSeconds(0.5f);

        SetCharacterState("idle");
    }

    IEnumerator StartAnimationTransition()
    {
        SetCharacterState("idle");

        yield return new WaitForSeconds(1f);

        SetCharacterState("circle");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Line"))
        {
        }

        if (other.gameObject.CompareTag("Push_line"))
        {
            // calculate force vector
            _force = transform.position - other.transform.position;
            // normalize force vector to get direction only and trim magnitude
            _force.Normalize();

            _playerRb.AddForce(_force.normalized * magnitude, ForceMode2D.Impulse);
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
        else if(state == "roll out")
        {
            SetAnimation(_rollout, false, 1f);
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
            if (_starHandler.levelIndex < 50)
            {
                PlayerPrefs.SetInt("level", _starHandler.levelIndex + 1);
            }
        }
        Debug.Log("Next level: " + PlayerPrefs.GetInt("level"));
    }
}
