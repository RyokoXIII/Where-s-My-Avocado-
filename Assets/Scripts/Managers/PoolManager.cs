using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Global Variables

    public GameObject particles;

    [Space(10f)]
    public List<Pool> poolList;
    public Dictionary<string, Queue<GameObject>> poolDict;

    StarHandler _starHandler;

    #endregion


    void Start()
    {
        _starHandler = StarHandler.Instance;

        CreatePooledObj();
    }

    void CreatePooledObj()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in poolList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);

                obj.transform.parent = particles.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDict.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " does not exist");
            return null;
        }

        // Spawn obj from pool
        GameObject objToSpawn = poolDict[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = pos;
        objToSpawn.transform.rotation = rot;

        // Recycle obj
        poolDict[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}
