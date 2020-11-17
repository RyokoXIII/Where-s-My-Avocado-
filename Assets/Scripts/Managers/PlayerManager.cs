using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Global Variables

    [Header("Sprite")]

    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] Sprite _playerHappySprite;
    [SerializeField] Sprite _playerSadSprite;
    [SerializeField] SpriteRenderer npcSprite;

    PoolManager _pooler;
    UIManager _uiManager;
    StarHandler _starHandler;

    Vector2 _force;
    public float magnitude = 5f;

    [Space(20f)]
    [SerializeField]
    Rigidbody2D _playerRb;
    [SerializeField]
    GameObject gameOverContainer;
    [SerializeField]
    NpcManager _npcManager;
    [SerializeField]
    Animator _npcAnim;

    int _starCount = 0;
    [HideInInspector]
    public int finalScore;

    [HideInInspector]
    public bool touchBoundary, hasFirstStar;

    #endregion


    void Start()
    {
        _pooler = PoolManager.Instance;
        _uiManager = UIManager.Instance;
        _starHandler = StarHandler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            _starCount += 1;
            finalScore = _starCount;

            hasFirstStar = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Npc"))
        {
            gameObject.transform.rotation = Quaternion.identity;
            _playerRb.constraints = RigidbodyConstraints2D.FreezeAll;

            UpdateCharactersState();

            // Game Over menu pop up Action callback
            StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
        }

        if (other.gameObject.CompareTag("Line"))
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

            // Sad face
            _playerSprite.sprite = _playerSadSprite;
            npcSprite.sprite = _npcManager.npcSadSprite;

            // Stop Animation
            _npcAnim.SetBool("isTouch", true);
            touchBoundary = true;

            // Game Over menu pop up Action callback
            StartCoroutine(_uiManager.GameOverRoutine(GameOverPopup));
        }
    }

    void UpdateCharactersState()
    {
        // Check if collected star of this level = 0
        if (hasFirstStar == false && (PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) == 0))
        {
            // Sad face
            _playerSprite.sprite = _playerSadSprite;
        }
        else
        {
            // Happy face
            _playerSprite.sprite = _playerHappySprite;
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
