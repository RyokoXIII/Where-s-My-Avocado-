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

    public GameObject sunsetCloudSpawner, beachCloudSpawner;

    [Header("Object Prefab")]
    [Space(30f)]
    public GameObject sunsetCloudPrefab;
    public GameObject beachCloudPrefab, pickUpParticlePrefab;
    public GameObject goalParticlePrefab, boundaryParticlePrefab;

    [Header("Number of Object to Spawn")]
    [Space(30f)]
    [Range(0, 50)]
    public int sunsetCloud;
    [Range(0, 50)]
    public int beachCloud, pickUpParticle;
    [Range(0, 50)]
    public int goalParticle, boundaryParticle;

    [Space(30f)]
    public List<GameObject> sunsetCloudList = new List<GameObject>();
    public List<GameObject> beachCloudList = new List<GameObject>();
    public List<GameObject> pickUpParticleList = new List<GameObject>();
    public List<GameObject> goalParticleList = new List<GameObject>();
    public List<GameObject> boundaryParticleList = new List<GameObject>();

    #endregion


    void Start()
    {
        if (StarHandler.Instance.levelIndex > 25)
        {
            beachCloudSpawner.SetActive(true);
            CreateBeachCloudList();
        }
        else
        {
            sunsetCloudSpawner.SetActive(true);
            CreateSunsetCloudList();
        }

        CreatePickUpParticleList();
        CreateGoalParticleList();
        CreateBoundaryParticleList();
    }

    void CreateSunsetCloudList()
    {
        for (int i = 0; i < sunsetCloud; i++)
        {
            GameObject cloud = Instantiate(sunsetCloudPrefab, sunsetCloudSpawner.transform.position, Quaternion.identity) as GameObject;

            cloud.transform.parent = sunsetCloudSpawner.transform;
            cloud.SetActive(false);
            sunsetCloudList.Add(cloud);
        }
    }

    void CreateBeachCloudList()
    {
        for (int i = 0; i < beachCloud; i++)
        {
            GameObject cloud = Instantiate(beachCloudPrefab, beachCloudSpawner.transform.position, Quaternion.identity) as GameObject;

            cloud.transform.parent = beachCloudSpawner.transform;
            cloud.SetActive(false);
            beachCloudList.Add(cloud);
        }
    }

    void CreatePickUpParticleList()
    {
        for (int i = 0; i < pickUpParticle; i++)
        {
            GameObject particle = Instantiate(pickUpParticlePrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

            particle.transform.parent = gameObject.transform;
            particle.SetActive(false);
            pickUpParticleList.Add(particle);
        }
    }

    void CreateGoalParticleList()
    {
        for (int i = 0; i < goalParticle; i++)
        {
            GameObject particle = Instantiate(goalParticlePrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

            particle.transform.parent = gameObject.transform;
            particle.SetActive(false);
            goalParticleList.Add(particle);
        }
    }

    void CreateBoundaryParticleList()
    {
        for (int i = 0; i < boundaryParticle; i++)
        {
            GameObject particle = Instantiate(boundaryParticlePrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

            particle.transform.parent = gameObject.transform;
            particle.SetActive(false);
            boundaryParticleList.Add(particle);
        }
    }
}
