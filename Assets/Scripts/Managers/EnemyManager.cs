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
    GameObject _coinFloatPrefab;

    #endregion

    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;

        // Animation
        _currentState = "1-idle";
        SetCharacterState(_currentState);
    }

    private void Update()
    {
        if (_coinFloatPrefab != null)
        {
            Vector3 targetPos = new Vector3(_coinFloatPrefab.transform.position.x, 
                _coinFloatPrefab.transform.position.y + 30f, _coinFloatPrefab.transform.position.z);

            _coinFloatPrefab.transform.position = Vector3.MoveTowards(_coinFloatPrefab.transform.position, targetPos, Time.deltaTime * 3f);
        }
    }

    // Check trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CreateParticles();

            _soundManager.enemySlashFX.Play();
            SetCharacterState("2-dead2");

            //Debug.Log("Enemy killed!");
            // Disable enemy after delay time
            StartCoroutine(DisabledObjectLateCall());
        }
    }

    void CreateParticles()
    {
        // Partice spawn position
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.5f);

        _pooler.SpawnFromPool("Slash Particle", pos, Quaternion.identity);
        _pooler.SpawnFromPool("BloodSplatSmall Particle", pos, Quaternion.identity);

        _coinFloatPrefab = _pooler.SpawnFromPool("CoinFloat Particle", transform.position, Quaternion.identity);
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
