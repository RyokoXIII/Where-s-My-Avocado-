using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyManager : MonoBehaviour, IAnimatable
{
    #region Global variables

    PoolManager _pooler;
    SoundManager _soundManager;
    [SerializeField] Camera main;
    [SerializeField] Coin _coinManager;
    [SerializeField] PlayerStats _playerStats;

    public int enemyTotalCoin;

    int _enemyGold = 5;
    int _currentLevel;


    [Space(10f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _dead;
    [SerializeField] string _currentState;

    string _currentAnimation;
    [SerializeField] GameObject _coinFloatPrefab;

    #endregion

    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;

        _currentLevel = PlayerPrefs.GetInt("levelID");

        // Animation
        _currentState = "1-idle";
        SetCharacterState(_currentState);
    }

    private void Update()
    {
        if (_coinFloatPrefab.activeInHierarchy == true)
        {
            Vector3 targetPos = new Vector3(_coinFloatPrefab.transform.position.x,
                _coinFloatPrefab.transform.position.y + 10f, _coinFloatPrefab.transform.position.z);

            _coinFloatPrefab.transform.position = Vector3.MoveTowards(_coinFloatPrefab.transform.position, targetPos, Time.deltaTime * 3f);
        }
    }

    // Check trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CreateParticles();
            EnemyGoldDrop();

            _soundManager.enemySlashFX.Play();
            SetCharacterState("2-dead2");

            //Debug.Log("Enemy killed!");
            // Disable enemy after delay time
            StartCoroutine(DisabledObjectLateCall());
        }
    }

    void EnemyGoldDrop()
    {
        int randomDrop = Random.Range(1, 4);

        if (_currentLevel > 1)
        {
            for (int i = 0; i < _currentLevel - 1; i++)
            {
                _enemyGold += (5 + randomDrop);
            }
            enemyTotalCoin += _enemyGold;
        }
        else
        {
            enemyTotalCoin += (_enemyGold + randomDrop);
        }

        _playerStats.currentExp += enemyTotalCoin;
    }

    void CreateParticles()
    {
        // Partice spawn position
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.5f);

        _pooler.SpawnFromPool("Slash Particle", pos, Quaternion.identity);
        _pooler.SpawnFromPool("BloodSplatSmall Particle", pos, Quaternion.identity);

        //_coinFloatPrefab = _pooler.SpawnFromPool("CoinFloat Particle", transform.position, Quaternion.identity);
        _coinFloatPrefab.SetActive(true);
        
    }

    IEnumerator DisabledObjectLateCall()
    {
        yield return new WaitForSeconds(0.8f);
        this.gameObject.SetActive(false);
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
        {
            return;
        }
        _skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        _currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state == "1-idle")
        {
            SetAnimation(_idle, true, 1f);
        }
        else if (state == "2-dead2")
        {
            SetAnimation(_dead, false, 1.15f);
        }
    }
}
