using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BossManager : MonoBehaviour, IAnimatable, IDamageable
{
    #region Global Variables

    [SerializeField]
    PlayerManager _playerManager;
    [SerializeField] GameObject _bloodSplatParticle;

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
    public HealthBar healthBarscript;
    public GameObject healthBar;

    float t = 0.0f;
    float threshold = 1f;

    #endregion

    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;

        // Animation
        _currentState = "1-idle";
        SetCharacterState(_currentState);
        _checkPlayAnim = false;

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
        bossDamage = _bossStats.attack;
        takeDamagePoint = _playerManager.playerDamage;

        maxHealth = _bossStats.maxHP;
        currentHealth = maxHealth;
        healthBarscript.SetMaxHealth(maxHealth);
    }

    void UpdateBossStats()
    {
        if (bossDamage < _bossStats.attack)
        {
            bossDamage = _bossStats.attack;
            takeDamagePoint = _playerManager.playerDamage;

            maxHealth = _bossStats.maxHP;
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

            if (currentHealth > damage)
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
        if (_playerManager.touchBoss == true && currentHealth > 0 && _playerManager.currentHealth > 0)
        {
            healthBar.SetActive(true);
            SetCharacterState("4-atk");
            TakeDamage(takeDamagePoint);
            CreateParticleEffect();
        }
        if (!_checkPlayAnim && currentHealth == 0)
        {
            _checkPlayAnim = true;
            StartCoroutine(DeadAnimationLateCall());
        }
        if(_playerManager.currentHealth == 0)
        {
            StartCoroutine(WinAnimationLateCall());
        }
    }

    IEnumerator DeadAnimationLateCall()
    {
        yield return new WaitForSeconds(0f);

        SetCharacterState("3-dead2");

        yield return new WaitForSeconds(0.1f);
        _bloodSplatParticle.SetActive(false);
        _soundManager.bossSlashFX.Play();

        yield return new WaitForSeconds(1.9f);
        healthBar.SetActive(false);
    }

    IEnumerator WinAnimationLateCall()
    {
        yield return new WaitForSeconds(0f);

        SetCharacterState("1-idle");

        yield return new WaitForSeconds(0.1f);
        _bloodSplatParticle.SetActive(false);

        yield return new WaitForSeconds(1.9f);
        healthBar.SetActive(false);
    }

    void CreateParticleEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 1f);
        _pooler.SpawnFromPool("Big Slash Particle", pos, Quaternion.identity);
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
        else if (state == "3-dead2")
        {
            SetAnimation(_dead, false, 1f);
        }
        else if (state == "4-atk")
        {
            SetAnimation(_attack, true, 1f);
        }
    }
}
