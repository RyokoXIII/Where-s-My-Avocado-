using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyManager : MonoBehaviour, IAnimatable
{
    #region Global variables
    PoolManager _pooler;
    SoundManager _soundManager;

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

    // Check trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Partice spawn position
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.5f);

            _pooler.SpawnFromPool("Slash Particle", pos, Quaternion.identity);
            _pooler.SpawnFromPool("BloodSplatSmall Particle", pos, Quaternion.identity);

            _soundManager.enemySlashFX.Play();
            SetCharacterState("2-dead");

            //Debug.Log("Enemy killed!");
            // Disable enemy after delay time
            StartCoroutine(DisabledObjectLateCall());
        }
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
        else if (state == "2-dead")
        {
            SetAnimation(_dead, false, 1f);
        }
    }
}
