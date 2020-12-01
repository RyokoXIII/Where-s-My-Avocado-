using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BossManager : MonoBehaviour, IAnimatable
{
    #region Global Variables

    [SerializeField]
    PlayerManager _playerManager;
    [SerializeField] BoxCollider2D _bossColl;

    PoolManager _pooler;
    SoundManager _soundManager;

    [Header("Animation")]
    [Space(10f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _dead;
    [SerializeField] string _currentState;

    string _currentAnimation;

    #endregion


    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;

        // Animation
        _currentState = "1-idle";
        SetCharacterState(_currentState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _bossColl.enabled = false;
            StartCoroutine(AnimationLateCall());
        }
    }

    IEnumerator AnimationLateCall()
    {
        yield return new WaitForSeconds(0.65f);

        SetCharacterState("2-dead");
        CreateParticleEffect();
        _soundManager.bossSlashFX.Play();
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
        else if (state == "2-dead")
        {
            SetAnimation(_dead, false, 1.2f);
        }
    }
}
