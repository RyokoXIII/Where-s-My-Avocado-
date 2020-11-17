using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    #region Global Variables

    [SerializeField]
    PlayerManager _playerManager;
    [SerializeField]
    Rigidbody2D _npcRb;
    [SerializeField]
    Animator _npcAnim;

    [Header("Sprite")][Space(10f)]
    [SerializeField] SpriteRenderer _npcSprite;
    public Sprite npcHappySprite;
    public Sprite npcSadSprite;

    [Header("Particle effect position")][Space(10f)]
    public float xPos = 6.76f;
    public float yPos = -2.39f;

    PoolManager _pooler;
    SoundManager _soundManager;
    StarHandler _starHandler;

    #endregion


    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;
        _starHandler = StarHandler.Instance;

        // Animation
        _npcAnim.SetBool("isTouch", false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Start heart particle
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.transform.rotation = Quaternion.identity;
            _npcRb.constraints = RigidbodyConstraints2D.FreezeAll;

            _soundManager.goalFX.Play();

            UpdateCharactersState();
            CreateParticleEffect();

            // Stop Animation after player touch npc
            _npcAnim.SetBool("isTouch", true);

        }
    }

    void UpdateCharactersState()
    {
        if (_playerManager.hasFirstStar == false && (PlayerPrefs.GetInt("lv" + _starHandler.levelIndex) == 0))
        {
            // Sad face
            _npcSprite.sprite = npcSadSprite;
        }
        else
        {
            // Happy face
            _npcSprite.sprite = npcHappySprite;
        }
    }

    void CreateParticleEffect()
    {
        Vector2 _heartPrefabPos = new Vector2(xPos, yPos);
        _pooler.SpawnFromPool("GoalParticle", _heartPrefabPos, Quaternion.identity);

        Debug.Log("HeartFX played!");
    }
}
