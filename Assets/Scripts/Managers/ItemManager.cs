using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    PoolManager _pooler;
    SoundManager _soundManager;


    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _pooler.SpawnFromPool("PickUpParticle", transform.position, Quaternion.identity);

            _soundManager.collectFX.Play();
            this.gameObject.SetActive(false);
        }
    }
}
