﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BossManager : MonoBehaviour, IAnimatable, IDamageable
{
    #region Global Variables

    [SerializeField]
    PlayerManager _playerManager;
    [SerializeField] Transform _playerPos;
    [SerializeField] GameObject _boundaryY;
    [SerializeField] GameObject _bloodSplatParticle;

    Vector3 _scaleChangeRight, _scaleChangeLeft;

    PoolManager _pooler;
    SoundManager _soundManager;

    [Header("Animation")]
    [Space(10f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _dead, _attack;
    [SerializeField] string _currentState;

    string _currentAnimation;
    public bool _checkPlayAnim;

    [Header("Boss Stats")]
    [Space(10f)]
    [SerializeField] BossStats _bossStats;
    public int bossAttack;
    public int currentHealth;
    public int maxHealth;
    public int takeDamagePoint;
    public HealthBar healthBarscript;
    public GameObject healthBar;

    [Header("Boss Coin Setup")]
    [Space(10f)]
    [SerializeField] GameObject _coinFloatPrefab;
    [SerializeField] PlayerStats _playerStats;
    public int goldDrop = 25;
    public int bossTotalCoin;

    int _currentLevel;

    float t = 0.0f;
    float threshold = 0.85f;


    #endregion

    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;

        _currentLevel = PlayerPrefs.GetInt("levelID");

        // Animation
        _currentState = "1-idle";
        SetCharacterState(_currentState);
        _checkPlayAnim = false;

        _scaleChangeRight = new Vector3(0.6f, 0.6f, 0.6f);
        _scaleChangeLeft = new Vector3(-0.6f, 0.6f, 0.6f);

        // Boss stats
        SetBossStats();
    }

    private void Update()
    {
        CheckBossGetKilled();

        if (_coinFloatPrefab.activeInHierarchy == true)
        {
            Vector3 targetPos = new Vector3(_coinFloatPrefab.transform.position.x,
                _coinFloatPrefab.transform.position.y + 30f, _coinFloatPrefab.transform.position.z);

            _coinFloatPrefab.transform.position = Vector3.MoveTowards(_coinFloatPrefab.transform.position, targetPos, Time.deltaTime * 2.5f);

            if(_coinFloatPrefab.transform.position.y > transform.position.y + 5f)
            {
                _coinFloatPrefab.SetActive(false);
            }
        }

        UpdateBossStats();
    }

    void SetBossStats()
    {
        bossAttack = _bossStats.baseAttack;
        takeDamagePoint = _playerManager.playerAttack;

        maxHealth = _bossStats.baseHealth;
        currentHealth = maxHealth;
        healthBarscript.SetMaxHealth(maxHealth);
    }

    void UpdateBossStats()
    {
        if (bossAttack < _bossStats.baseAttack)
        {
            bossAttack = _bossStats.baseAttack;
            takeDamagePoint = _playerManager.playerAttack;

            maxHealth = _bossStats.baseHealth;
            currentHealth = maxHealth;
            healthBarscript.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        t += Time.deltaTime;

        if (t >= threshold)
        {
            t = 0.0f;

            if (currentHealth >= damage)
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

    void CheckBossGetKilled()
    {
        if (Vector2.Distance(transform.position, _playerPos.position) < 3f)
        {
            if (transform.position.x < _playerPos.position.x)
            {
                transform.localScale = _scaleChangeLeft;
            }
            else
            {
                transform.localScale = _scaleChangeRight;
            }
        }

        if (_playerManager.touchBoss == true && currentHealth > 0 && _playerManager.currentHealth > 0)
        {
            healthBar.SetActive(true);
            SetCharacterState("4-atk");
            TakeDamage(takeDamagePoint);
            CreateBloodParticleEffect();
        }
        if (!_checkPlayAnim && currentHealth == 0)
        {
            _checkPlayAnim = true;
            _bloodSplatParticle.SetActive(false);

            StartCoroutine(DeadAnimationLateCall());
        }
        if (_playerManager.currentHealth == 0)
        {
            _bloodSplatParticle.SetActive(false);
            StartCoroutine(WinAnimationLateCall());
        }
    }

    IEnumerator DeadAnimationLateCall()
    {
        yield return new WaitForSeconds(0f);

        SetCharacterState("2-dead");

        BossGoldDrop();
        CreateSlashParticleEffect();

        _soundManager.bossSlashFX.Play();

        yield return new WaitForSeconds(0.5f);
        healthBar.SetActive(false);
    }

    IEnumerator WinAnimationLateCall()
    {
        yield return new WaitForSeconds(0f);

        SetCharacterState("1-idle");

        yield return new WaitForSeconds(0.5f);
        healthBar.SetActive(false);
    }

    void BossGoldDrop()
    {
        int randomDrop = Random.Range(5, 11);

        if (_currentLevel > 1)
        {
            for (int i = 0; i < _currentLevel - 1; i++)
            {
                goldDrop += (25 + randomDrop);
            }
            bossTotalCoin += goldDrop;
        }
        else
        {
            bossTotalCoin += (goldDrop + randomDrop);
        }

        _playerStats.currentExp += bossTotalCoin;
    }

    void CreateSlashParticleEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 1f);
        _pooler.SpawnFromPool("Big Slash Particle", pos, Quaternion.identity);

        //_coinFloatPrefab = _pooler.SpawnFromPool("CoinFloat Particle", pos, Quaternion.identity);
        _coinFloatPrefab.SetActive(true);
    }

    void CreateBloodParticleEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 1f);
        GameObject obj = _pooler.SpawnFromPool("BloodSplatWide Particle", pos, Quaternion.identity);
        _bloodSplatParticle = obj;
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
            return;

        _skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        _currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state == "1-idle")
        {
            SetAnimation(_idle, true, 1f);
        }
        else if (state == "2-dead")
        {
            SetAnimation(_dead, false, 1f);
        }
        else if (state == "4-atk")
        {
            SetAnimation(_attack, true, 0.7f);
        }
    }
}
