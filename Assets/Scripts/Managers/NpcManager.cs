using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    #region Global Variables

    [SerializeField]
    ParticleSystem _heartFlowFXPrefab = null;
    [SerializeField]
    GameObject player;
    Rigidbody2D _rb;
    Animator _anim;

    [Header("Sprite")][Space(10f)]
    public Sprite happyImg;
    public Sprite sadImg;

    [Header("Particle effect position")][Space(10f)]
    public float xPos = 6.76f;
    public float yPos = -2.39f;

    PlayerManager _playerManager;

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

            UpdateCharactersState();
            CreateParticleEffect();

            // Stop Animation after player touch npc
            _anim.SetBool("isTouch", true);

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

    void CreateParticleEffect()
    {
        Vector2 _heartPrefabPos = new Vector2(xPos, yPos);

        for (int i = 0; i < PoolManager.Instance.goalParticleList.Count; i++)
        {
            if (PoolManager.Instance.goalParticleList[i].activeInHierarchy == false)
            {
                PoolManager.Instance.goalParticleList[i].SetActive(true);
                PoolManager.Instance.goalParticleList[i].transform.position = _heartPrefabPos;
                break;
            }
        }
        Debug.Log("HeartFX played!");
    }
}
