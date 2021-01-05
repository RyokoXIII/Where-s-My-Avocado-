using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BossManager : MonoBehaviour, IAnimatable, IDamageable
{
    #region Global Variables

    [SerializeField]
    PlayerManager _playerManager;

    PoolManager _pooler;
    SoundManager _soundManager;

    [Header("Animation")]
    [Space(10f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _dead, _attack;
    [SerializeField] string _currentState;

    string _currentAnimation;
    public bool _checkPlayAnim;

    // Player Stats
    public int hitPoint = 20;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBarscript;
    public GameObject healthBar;

    private float t = 0.0f;
    private float threshold = 1f;

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
        SetHealthStats();
    }

    private void Update()
    {
        CheckBossGetKilled();
    }

    void SetHealthStats()
    {
        currentHealth = maxHealth;
        healthBarscript.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        t += Time.deltaTime;

        if (t >= threshold)
        {
            t = 0.0f;
            currentHealth -= damage;
            healthBarscript.SetCurrentHealth(currentHealth);
        }
    }

    void CheckBossGetKilled()
    {
        if (_playerManager.touchBoss == true && currentHealth > 0)
        {
            healthBar.SetActive(true);
            SetCharacterState("4-atk");
            TakeDamage(hitPoint);
        }
        else if (!_checkPlayAnim && currentHealth == 0)
        {
            _checkPlayAnim = true;
            StartCoroutine(AnimationLateCall());
        }
    }

    IEnumerator AnimationLateCall()
    {
        yield return new WaitForSeconds(0f);

        SetCharacterState("3-dead2");

        //yield return new WaitForSeconds(0f);
        CreateParticleEffect();
        _soundManager.bossSlashFX.Play();

        yield return new WaitForSeconds(2f);
        healthBar.SetActive(false);
    }

    void CreateParticleEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 1f);
        _pooler.SpawnFromPool("Big Slash Particle", pos, Quaternion.identity);
        _pooler.SpawnFromPool("BloodSplatWide Particle", pos, Quaternion.identity);
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
