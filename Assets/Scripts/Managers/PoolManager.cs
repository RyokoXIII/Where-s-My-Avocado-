using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Singleton

    static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("PoolManager does not exist!");

            return _instance;
        }
    }

    // Initialize instance to this class
    private void Awake()
    {
        _instance = this;
    }

    #endregion

    #region Global Variables

    public GameObject player, npc, star, sunsetCloudSpawner, cloudSpawner;

    [Header("Object Prefab")]
    [Space(30f)]
    public GameObject sunsetCloudPrefab;
    public GameObject beachCloudPrefab, pickUpParticlePrefab;
    public GameObject goalParticlePrefab, boundaryParticlePrefab;

    [Header("Number of Object to Spawn")]
    [Space(30f)][Range(0,50)]
    public int sunsetCloud;
    [Range(0, 50)]
    public int beachCloud, pickUpParticle;
    [Range(0, 50)]
    public int goalParticle, boundaryParticle;

    [Space(30f)]
    public List<GameObject> cloudList = new List<GameObject>();

    #endregion


    void Start()
    {
        for (int i = 0; i < sunsetCloud; i++)
        {
            GameObject cloud = Instantiate(sunsetCloudPrefab, sunsetCloudSpawner.transform.position, Quaternion.identity) as GameObject;

            cloud.transform.parent = sunsetCloudSpawner.transform;
            cloud.SetActive(false);
            cloudList.Add(cloud);
        }
    }
}
