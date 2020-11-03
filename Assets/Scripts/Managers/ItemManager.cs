using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _collectParticle = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(_collectParticle, transform.position, Quaternion.identity);
            SoundManager.Instance.collectFX.Play();
            
            this.gameObject.SetActive(false);
        }
    }
}
