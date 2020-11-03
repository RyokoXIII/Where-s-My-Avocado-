using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    #region Global Variables

    public GameObject[] cloudPrefabs;
    public float yPos1, yPos2;

    public float startSpawnTime = 5f;
    public float repeatRate = 10f;

    #endregion


    void Start()
    {
        InvokeRepeating("SpawnRandomCloud", startSpawnTime, repeatRate);
    }

    void SpawnRandomCloud()
    {
        float randomYPos = Random.Range(yPos1, yPos2);
        Vector2 randomPos = new Vector2(transform.position.x, randomYPos);

        int randomIndex = Random.Range(0, cloudPrefabs.Length);

        Instantiate(cloudPrefabs[randomIndex], randomPos, Quaternion.identity);
    }
}
