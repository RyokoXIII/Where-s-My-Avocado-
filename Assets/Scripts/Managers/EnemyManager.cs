using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyManager : MonoBehaviour, IAnimatable
{
    PoolManager _pooler;
    SoundManager _soundManager;

    [Space(10f)]
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    [SerializeField] AnimationReferenceAsset _idle, _dead;
    [SerializeField] string _currentState;

    string _currentAnimation;

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
        if (other.CompareTag("Player"))
        {
            _pooler.SpawnFromPool("PickUpParticle", transform.position, Quaternion.identity);

            _soundManager.collectFX.Play();
            SetCharacterState("2-dead");

            StartCoroutine(LateCall());
        }
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(1.2f);
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
        else if (state == "2-dead")
        {
            SetAnimation(_dead, false, 1f);
        }
    }
}
