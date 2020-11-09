using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Global Variables

    [Header("Sprite")][SerializeField]
    Sprite happyImg;
    [SerializeField] Sprite sadImg;

    Rigidbody2D _rb;
    Vector2 _force;
    NpcManager _npcManager;

    public float magnitude = 5f;

    [Space(20f)]
    [SerializeField]
    ParticleSystem _collideBoundParticle;
    [SerializeField]
    GameObject gameOverContainer, npc;
    [SerializeField]
    SpriteRenderer npcSprite;

    Animator _anim;

    int _starCount = 0;
    [HideInInspector]
    public int finalScore;

    [HideInInspector]
    public bool touchBoundary, hasFirstStar;

    #endregion


    void Start()
    {

        _rb = GetComponent<Rigidbody2D>();
        npcSprite = npc.GetComponent<SpriteRenderer>();
        _npcManager = npc.GetComponent<NpcManager>();
        _anim = npc.GetComponent<Animator>();
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
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;

            UpdateCharactersState();

            // Game Over menu pop up Action callback
            StartCoroutine(UIManager.Instance.GameOverRoutine(GameOverPopup));
        }

        if (other.gameObject.CompareTag("Line"))
        {
            // calculate force vector
            _force = transform.position - other.transform.position;
            // normalize force vector to get direction only and trim magnitude
            _force.Normalize();

            _rb.AddForce(_force.normalized * magnitude, ForceMode2D.Impulse);
        }

        if (other.gameObject.CompareTag("Boundary"))
        {
            CreateParticleEffect();

            gameObject.transform.rotation = Quaternion.identity;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;

            // Sad face
            GetComponent<SpriteRenderer>().sprite = sadImg;
            npcSprite.sprite = _npcManager.sadImg;

            // Stop Animation
            _anim.SetBool("isTouch", true);

            touchBoundary = true;

            // Game Over menu pop up Action callback
            StartCoroutine(UIManager.Instance.GameOverRoutine(GameOverPopup));
        }
    }

    void UpdateCharactersState()
    {
        // Check if collected star of this level = 0
        if (hasFirstStar == false && (PlayerPrefs.GetInt("lv" + StarHandler.Instance.levelIndex) == 0))
        {
            // Sad face
            GetComponent<SpriteRenderer>().sprite = sadImg;
        }
        else
        {
            // Happy face
            GetComponent<SpriteRenderer>().sprite = happyImg;
        }
    }

    void CreateParticleEffect()
    {
        for (int i = 0; i < PoolManager.Instance.boundaryParticleList.Count; i++)
        {
            if (PoolManager.Instance.boundaryParticleList[i].activeInHierarchy == false)
            {
                PoolManager.Instance.boundaryParticleList[i].SetActive(true);
                PoolManager.Instance.boundaryParticleList[i].transform.position = transform.position;
                break;
            }
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
        StarHandler.Instance.StarAchieved(finalScore);

        if (finalScore > PlayerPrefs.GetInt("lv" + StarHandler.Instance.levelIndex) && touchBoundary == false)
        {
            PlayerPrefs.SetInt("lv" + StarHandler.Instance.levelIndex, finalScore);
            SaveNextLevel();
        }
        Debug.Log("Collected Stars: " + PlayerPrefs.GetInt("lv" + StarHandler.Instance.levelIndex));
    }

    // Save newest unlocked level
    void SaveNextLevel()
    {
        if (PlayerPrefs.GetInt("level") <= StarHandler.Instance.levelIndex)
        {
            if (StarHandler.Instance.levelIndex < 50)
            {
                PlayerPrefs.SetInt("level", StarHandler.Instance.levelIndex + 1);
            }
        }
        Debug.Log("Next level: " + PlayerPrefs.GetInt("level"));
    }
}
