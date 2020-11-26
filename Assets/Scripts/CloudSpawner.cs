using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    #region Global Variables

    PoolManager _pooler;
    public float yPos1, yPos2;

    public float startSpawnTime = 5f;
    public float repeatRate = 10f;

    Vector2 _randomPos;

    #endregion


    void Start()
    {
        _pooler = PoolManager.Instance;

        //InvokeRepeating("SpawnRandomCloud", startSpawnTime, repeatRate);
    }

    void SpawnRandomCloud()
    {
        float randomYPos = Random.Range(yPos1, yPos2);
        _randomPos = new Vector2(transform.position.x, randomYPos);

        _pooler.SpawnFromPool("BeachCloud", _randomPos, Quaternion.identity);
    }

    //void SpawnBeachClouds()
    //{
    //    _pooler.SpawnFromPool("BeachCloud", _randomPos, Quaternion.identity);
    //}
}
