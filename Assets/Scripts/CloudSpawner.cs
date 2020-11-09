using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    #region Global Variables

    public float yPos1, yPos2;

    public float startSpawnTime = 5f;
    public float repeatRate = 10f;

    Vector2 _randomPos;

    #endregion


    void Start()
    {
        InvokeRepeating("SpawnRandomCloud", startSpawnTime, repeatRate);
    }

    void SpawnRandomCloud()
    {
        float randomYPos = Random.Range(yPos1, yPos2);
        _randomPos = new Vector2(transform.position.x, randomYPos);

        if(PoolManager.Instance.sunsetCloudSpawner.activeInHierarchy == true)
        {
            SpawnSunsetClouds();
        }
        else
        {
            SpawnBeachClouds();
        }
    }

    void SpawnSunsetClouds()
    {
        for (int i = 0; i < PoolManager.Instance.sunsetCloudList.Count; i++)
        {
            if (PoolManager.Instance.sunsetCloudList[i].activeInHierarchy == false)
            {
                PoolManager.Instance.sunsetCloudList[i].SetActive(true);
                PoolManager.Instance.sunsetCloudList[i].transform.position = _randomPos;
                break;
            }
        }
    }

    void SpawnBeachClouds()
    {
        for (int i = 0; i < PoolManager.Instance.beachCloudList.Count; i++)
        {
            if (PoolManager.Instance.beachCloudList[i].activeInHierarchy == false)
            {
                PoolManager.Instance.beachCloudList[i].SetActive(true);
                PoolManager.Instance.beachCloudList[i].transform.position = _randomPos;
                break;
            }
        }
    }
}
