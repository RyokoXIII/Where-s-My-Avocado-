using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BossManager : MonoBehaviour, IAnimatable, IDamageable
{
    #region Global Variables

    [SerializeField]
    PlayerManager _playerManager;
    [SerializeField] Transform _playerPos;
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
    public int bossDamage;
    public int currentHealth;
    public int maxHealth;
    public int takeDamagePoint;
    public int goldDrop;
    public HealthBar healthBarscript;
    public GameObject healthBar;

    float t = 0.0f;
    float threshold = 0.85f;

    #endregion

    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;

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

        UpdateBossStats();
    }

    void SetBossStats()
    {
        bossDamage = _bossStats.baseAttack;
        takeDamagePoint = _playerManager.playerDamage;

        maxHealth = _bossStats.baseHealth;
        currentHealth = maxHealth;
        healthBarscript.SetMaxHealth(maxHealth);
    }

    void UpdateBossStats()
    {
        if (bossDamage < _bossStats.baseAttack)
        {
            bossDamage = _bossStats.baseAttack;
            takeDamagePoint = _playerManager.playerDamage;

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

    void CreateSlashParticleEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 1f);
        _pooler.SpawnFromPool("Big Slash Particle", pos, Quaternion.identity);
        _pooler.SpawnFromPool("BossGoldParticle", pos, Quaternion.LookRotation(Vector3.up));
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
            SetAnimation(_attack, true, 1f);
        }
    }
}
