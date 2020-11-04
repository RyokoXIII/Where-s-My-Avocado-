using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    #region Global Variables

    [SerializeField]
    ParticleSystem _heartFlowFXPrefab = null;
    Rigidbody2D _rb;
    Animator _anim;

    public Sprite happyImg, sadImg;

    public float xPos = 6.76f;
    public float yPos = -2.39f;

    PlayerManager _playerManager;
    [SerializeField]
    GameObject player;

    #endregion


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerManager = player.GetComponent<PlayerManager>();

        _anim = GetComponent<Animator>();
        _anim.SetBool("isTouch", false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Start heart particle
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.transform.rotation = Quaternion.identity;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;

            SoundManager.Instance.goalFX.Play();
            Vector2 _heartPrefabPos = new Vector2(xPos, yPos);

            UpdateCharactersState();

            // Stop Animation after player touch npc
            _anim.SetBool("isTouch", true);

            Instantiate(_heartFlowFXPrefab, _heartPrefabPos, Quaternion.identity);
            Debug.Log("HeartFX played!");
        }
    }

    void UpdateCharactersState()
    {
        if (_playerManager.hasFirstStar == false && (PlayerPrefs.GetInt("lv" + StarHandler.Instance.levelIndex) == 0))
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
}
