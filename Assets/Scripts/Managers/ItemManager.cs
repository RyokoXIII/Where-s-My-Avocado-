using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < PoolManager.Instance.pickUpParticleList.Count; i++)
            {
                if (PoolManager.Instance.pickUpParticleList[i].activeInHierarchy == false)
                {
                    PoolManager.Instance.pickUpParticleList[i].SetActive(true);
                    PoolManager.Instance.pickUpParticleList[i].transform.position = transform.position;
                    break;
                }
            }
            SoundManager.Instance.collectFX.Play();

            this.gameObject.SetActive(false);
        }
    }
}
