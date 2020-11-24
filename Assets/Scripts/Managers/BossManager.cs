using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BossManager : MonoBehaviour, IAnimatable
{
    #region Global Variables

    [SerializeField]
    PlayerManager _playerManager;

    [Header("Particle effect position")]
    [Space(10f)]
    public float xPos = 6.76f;
    public float yPos = -2.39f;

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
            SetCharacterState("2-dead");
            _soundManager.goalFX.Play();

            CreateParticleEffect();
        }
    }

    void CreateParticleEffect()
    {
        Vector2 _heartPrefabPos = new Vector2(xPos, yPos);
        _pooler.SpawnFromPool("GoalParticle", _heartPrefabPos, Quaternion.identity);

        Debug.Log("HeartFX played!");
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
    }
}
