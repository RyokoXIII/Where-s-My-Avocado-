using System.Collections;
using System.Collections.Generic;
using System.IO;
using Spine.Unity;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Global Variables

    [Header("Data Files")]
    [Space(10f)]
    [SerializeField] int[] dataList;

    [Header("Characters")]
    [Space(10f)]
    [SerializeField] Transform _playerPos;
    [SerializeField] Transform _bossPos;
    [SerializeField] BossManager _bossManager;


    [Header("Grounds")]
    [Space(10f)]
    [SerializeField] GameObject _foregroundContainer;
    [SerializeField] GameObject[] levelForegroundList;

    [Header("Enemies")]
    [Space(10f)]
    [SerializeField] GameObject[] goblinList;
    [SerializeField] GameObject[] batList;

    [Header("Obstacles")]
    [Space(10f)]
    [SerializeField] GameObject _obstaclesContainer;
    [SerializeField] LineManager _lineManager;
    [SerializeField] GameObject _deadZonePrefab;
    [SerializeField] GameObject _roundLogPrefab, _seeSawPrefab, _woodPrefab, _bigWoodPrefab, _cloudPrefab;

    [Header("Tutorial")]
    [Space(10f)]
    [SerializeField] GameObject _tutorial;
    public GameObject _tutorialHand1, _tutorialHand2, _tutorialHand3;
    public GameObject _lineTut1, _lineTut2, _lineTut3;

    LevelData _loadLevelData;
    string _json;
    string _path;

    // List of Enemy Positions
    List<float> goblinPosXList;
    List<float> goblinPosYList;

    List<float> batPosXList;
    List<float> batPosYList;

    // List of Obstacle Positions
    List<float> cloudPosXList;
    List<float> cloudPosYList;

    List<float> roundLogXList;
    List<float> roundLogYList;

    List<float> woodPosXList;
    List<float> woodPosYList;

    List<float> seesawPosXList;
    List<float> seesawPosYList;
    List<float> seesawRotateZList;

    List<float> bigWoodPosXList;
    List<float> bigWoodPosYList;
    List<float> bigWoodScaleXList;
    List<float> bigWoodScaleYList;
    List<float> bigWoodRotateZList;

    #endregion

    private void Start()
    {
        LoadDataFile();
    }

    void LoadDataFile()
    {
        if (PlayerPrefs.GetInt("levelID") > 0)
        {
            for (int i = 0; i < dataList.Length; i++)
            {
                if (dataList[i].Equals(PlayerPrefs.GetInt("levelID")))
                {
                    //_path = Application.dataPath + "/Resources/" + dataList[i].ToString() + ".json";
                    _path = dataList[i].ToString();
                }
            }
        }
        else
        {
            _path = dataList[0].ToString();
        }

        Debug.Log(_path);

        LoadLevelData(_path);
    }

    void LoadLevelData(string path)
    {
        //_json = File.ReadAllText(path);

        TextAsset file = Resources.Load(path) as TextAsset;
        _json = file.text;

        Debug.Log(_json);
        // Get all data from json
        _loadLevelData = JsonUtility.FromJson<LevelData>(_json);

        // Load Level data
        LoadCharacterData();
        LoadForegroundData();
        LoadEnemiesData();
        LoadObstacles();
        LoadTutorial();

    }

    void LoadCharacterData()
    {
        // Player pos
        Vector3 newPlayerPos = new Vector3(_loadLevelData.playerPosX, _loadLevelData.playerPosY, 0f);
        _playerPos.transform.position = newPlayerPos;

        // Boss pos
        Vector3 newBossPos = new Vector3(_loadLevelData.bossPosX, _loadLevelData.bossPosY, 0f);
        _bossPos.transform.position = newBossPos;

        if (_loadLevelData.rotatePlayer == true)
        {
            _playerPos.transform.localScale = new Vector3(-_playerPos.transform.localScale.x, _playerPos.transform.localScale.y,
                _playerPos.transform.localScale.z);
        }
        if (_loadLevelData.rotateBoss == true)
        {
            _bossPos.transform.localScale = new Vector3(-_bossPos.transform.localScale.x, _bossPos.transform.localScale.y,
                _bossPos.transform.localScale.z);
        }
    }

    void LoadForegroundData()
    {
        for (int i = 0; i < levelForegroundList.Length; i++)
        {
            if (_loadLevelData.levelID.Equals(levelForegroundList[i].name))
            {
                // Load Level foreground
                GameObject ground = Instantiate(levelForegroundList[i], _foregroundContainer.transform.position, Quaternion.identity);
                ground.transform.parent = _foregroundContainer.transform;
            }
        }
    }

    void LoadEnemiesData()
    {
        LoadEnemiesPosData();

        // Goblin Data
        if (_loadLevelData.hasGoblin == true)
        {
            for (int i = 0; i < _loadLevelData.goblinNum; i++)
            {
                Vector2 newPos = new Vector2(goblinPosXList[i], goblinPosYList[i]);
                goblinList[i].transform.position = newPos;
                goblinList[i].SetActive(true);
            }
        }
        // Bat data
        if (_loadLevelData.hasBat == true)
        {
            for (int i = 0; i < _loadLevelData.batNum; i++)
            {
                Vector2 newPos = new Vector2(batPosXList[i], batPosYList[i]);
                batList[i].transform.position = newPos;
                batList[i].SetActive(true);
            }
        }

        // Rotate Data
        if (_loadLevelData.rotateBat == true)
        {
            for (int i = 0; i < _loadLevelData.batNum; i++)
            {
                batList[i].transform.localScale = new
                   Vector3(-batList[i].transform.localScale.x, batList[i].transform.localScale.y, batList[i].transform.localScale.z);
            }
        }
        if (_loadLevelData.rotateGoblin == true)
        {
            for (int i = 0; i < _loadLevelData.goblinNum; i++)
            {
                goblinList[i].transform.localScale = new
                   Vector3(-goblinList[i].transform.localScale.x, goblinList[i].transform.localScale.y, goblinList[i].transform.localScale.z);
            }
        }
    }

    void LoadEnemiesPosData()
    {
        goblinPosXList = new List<float>();
        goblinPosYList = new List<float>();

        goblinPosXList.Add(_loadLevelData.goblinPosX_1);
        goblinPosXList.Add(_loadLevelData.goblinPosX_2);
        goblinPosXList.Add(_loadLevelData.goblinPosX_3);

        goblinPosYList.Add(_loadLevelData.goblinPosY_1);
        goblinPosYList.Add(_loadLevelData.goblinPosY_2);
        goblinPosYList.Add(_loadLevelData.goblinPosY_3);

        batPosXList = new List<float>();
        batPosYList = new List<float>();

        batPosXList.Add(_loadLevelData.batPosX_1);
        batPosXList.Add(_loadLevelData.batPosX_2);
        batPosXList.Add(_loadLevelData.batPosX_3);

        batPosYList.Add(_loadLevelData.batPosY_1);
        batPosYList.Add(_loadLevelData.batPosY_2);
        batPosYList.Add(_loadLevelData.batPosY_3);
    }

    void LoadTutorial()
    {
        if (_loadLevelData.tutorial == true)
        {
            _tutorial.SetActive(true);

            switch (_loadLevelData.levelID)
            {
                case "1":
                    _tutorialHand1.SetActive(true);
                    _lineTut1.SetActive(true);
                    break;
                case "2":
                    _tutorialHand2.SetActive(true);
                    _lineTut2.SetActive(true);
                    break;
                case "5":
                    _tutorialHand3.SetActive(true);
                    _lineTut3.SetActive(true);
                    break;
            }
        }
    }

    void LoadObstacles()
    {
        if (_loadLevelData.roundLog == true)
        {
            LoadRoundLogPosData();

            for (int i = 0; i < _loadLevelData.roundLogNum; i++)
            {
                Vector2 newPos = new Vector2(roundLogXList[i], roundLogYList[i]);

                GameObject obj = Instantiate(_roundLogPrefab, newPos, Quaternion.identity);

                // Add obj to roundlog list in LineManager
                Rigidbody2D roundLogRb = obj.GetComponent<Rigidbody2D>();
                _lineManager.roundLogRbs.Add(roundLogRb);

                obj.transform.parent = _obstaclesContainer.transform;

                Vector3 newScale = new Vector3(_loadLevelData.roundLogScaleX, _loadLevelData.roundLogScaleY, _loadLevelData.roundLogScaleZ);
                obj.transform.localScale = newScale;

            }

        }

        if (_loadLevelData.wood == true)
        {
            LoadWoodPosData();

            for (int i = 0; i < _loadLevelData.woodNum; i++)
            {
                Vector2 newPos = new Vector2(woodPosXList[i], woodPosYList[i]);

                GameObject obj = Instantiate(_woodPrefab, newPos, Quaternion.identity);
                obj.transform.parent = _obstaclesContainer.transform;
            }
        }

        if (_loadLevelData.deadZone == true)
        {
            Vector2 newPos = new Vector2(_loadLevelData.deadZonePosX, _loadLevelData.deadZonePosY);

            GameObject obj = Instantiate(_deadZonePrefab, newPos, Quaternion.identity);

            Vector3 newScale = new Vector3(_loadLevelData.deadZoneScaleX, _loadLevelData.deadZoneScaleY, obj.transform.localScale.z);
            obj.transform.localScale = newScale;
        }

        if (_loadLevelData.seesaw == true)
        {
            LoadSeesawPosData();

            for (int i = 0; i < _loadLevelData.seesawNum; i++)
            {
                Vector2 newPos = new Vector2(seesawPosXList[i], seesawPosYList[i]);
                Quaternion newRotate = Quaternion.Euler(0, 0, seesawRotateZList[i]);

                GameObject obj = Instantiate(_seeSawPrefab, newPos, newRotate);
                obj.transform.parent = _obstaclesContainer.transform;

                Vector3 newScale = new Vector3(_loadLevelData.seesawScaleX, _loadLevelData.seesawScaleY, obj.transform.localScale.z);
                obj.GetComponentInChildren<Transform>().Find("see_saw").localScale = newScale;
            }
        }

        if (_loadLevelData.bigWood == true)
        {
            LoadBigWoodPosData();

            for (int i = 0; i < _loadLevelData.bigWoodNum; i++)
            {
                Vector2 newPos = new Vector2(bigWoodPosXList[i], bigWoodPosYList[i]);
                Quaternion newRotate = Quaternion.Euler(0, 0, bigWoodRotateZList[i]);

                GameObject obj = Instantiate(_bigWoodPrefab, newPos, newRotate);

                // Add obj to bigWood list in LineManager
                Rigidbody2D bigWoodRb = obj.GetComponent<Rigidbody2D>();
                _lineManager.bigWoodRbs.Add(bigWoodRb);

                obj.transform.parent = _obstaclesContainer.transform;

                Vector3 newScale = new Vector3(bigWoodScaleXList[i], bigWoodScaleYList[i], obj.transform.localScale.z);
                obj.transform.localScale = newScale;
            }
        }

        if (_loadLevelData.woodNest == true)
        {
            LoadBigWoodPosData();

            for (int i = 0; i < _loadLevelData.woodNestNum; i++)
            {
                Vector2 newPos = new Vector2(bigWoodPosXList[i], bigWoodPosYList[i]);
                Quaternion newRotate = Quaternion.Euler(0, 0, bigWoodRotateZList[i]);

                GameObject obj = Instantiate(_bigWoodPrefab, newPos, newRotate);

                // Add obj to smallWood list in LineManager
                Rigidbody2D smallWoodRb = obj.GetComponent<Rigidbody2D>();
                _lineManager.smallWoodRbs.Add(smallWoodRb);

                obj.transform.parent = _obstaclesContainer.transform;

                Vector3 newScale = new Vector3(bigWoodScaleXList[i], bigWoodScaleYList[i], obj.transform.localScale.z);
                obj.transform.localScale = newScale;
            }
        }

        if (_loadLevelData.cloud == true)
        {
            LoadCloudPosData();

            for (int i = 0; i < _loadLevelData.cloudNum; i++)
            {
                Vector2 newPos = new Vector2(cloudPosXList[i], cloudPosYList[i]);

                GameObject obj = Instantiate(_cloudPrefab, newPos, Quaternion.identity);
                obj.transform.parent = _obstaclesContainer.transform;

                SkeletonAnimation objAnim = obj.GetComponent<SkeletonAnimation>();
                _lineManager.skeletonAnimationList.Add(objAnim);
            }
        }
    }

    void LoadCloudPosData()
    {
        cloudPosXList = new List<float>();
        cloudPosYList = new List<float>();

        cloudPosXList.Add(_loadLevelData.cloudPosX_1);
        cloudPosXList.Add(_loadLevelData.cloudPosX_2);
        cloudPosXList.Add(_loadLevelData.cloudPosX_3);
        cloudPosXList.Add(_loadLevelData.cloudPosX_4);
        cloudPosXList.Add(_loadLevelData.cloudPosX_5);
        cloudPosXList.Add(_loadLevelData.cloudPosX_6);

        cloudPosYList.Add(_loadLevelData.cloudPosY_1);
        cloudPosYList.Add(_loadLevelData.cloudPosY_2);
        cloudPosYList.Add(_loadLevelData.cloudPosY_3);
        cloudPosYList.Add(_loadLevelData.cloudPosY_4);
        cloudPosYList.Add(_loadLevelData.cloudPosY_5);
        cloudPosYList.Add(_loadLevelData.cloudPosY_6);
    }

    void LoadRoundLogPosData()
    {
        roundLogXList = new List<float>();
        roundLogYList = new List<float>();

        roundLogXList.Add(_loadLevelData.roundLogPosX_1);
        roundLogXList.Add(_loadLevelData.roundLogPosX_2);
        roundLogXList.Add(_loadLevelData.roundLogPosX_3);
        roundLogXList.Add(_loadLevelData.roundLogPosX_4);
        roundLogXList.Add(_loadLevelData.roundLogPosX_5);

        roundLogYList.Add(_loadLevelData.roundLogPosY_1);
        roundLogYList.Add(_loadLevelData.roundLogPosY_2);
        roundLogYList.Add(_loadLevelData.roundLogPosY_3);
        roundLogYList.Add(_loadLevelData.roundLogPosY_4);
        roundLogYList.Add(_loadLevelData.roundLogPosY_5);
    }

    void LoadWoodPosData()
    {
        woodPosXList = new List<float>();
        woodPosYList = new List<float>();

        woodPosXList.Add(_loadLevelData.woodPosX1);
        woodPosXList.Add(_loadLevelData.woodPosX2);
        woodPosXList.Add(_loadLevelData.woodPosX3);
        woodPosXList.Add(_loadLevelData.woodPosX4);
        woodPosXList.Add(_loadLevelData.woodPosX5);
        woodPosXList.Add(_loadLevelData.woodPosX6);

        Debug.Log(woodPosXList.Count.ToString());

        woodPosYList.Add(_loadLevelData.woodPosY1);
        woodPosYList.Add(_loadLevelData.woodPosY2);
        woodPosYList.Add(_loadLevelData.woodPosY3);
        woodPosYList.Add(_loadLevelData.woodPosY4);
        woodPosYList.Add(_loadLevelData.woodPosY5);
        woodPosYList.Add(_loadLevelData.woodPosY6);
    }

    void LoadSeesawPosData()
    {
        seesawPosXList = new List<float>();
        seesawPosYList = new List<float>();
        seesawRotateZList = new List<float>();

        seesawPosXList.Add(_loadLevelData.seesawPosX1);
        seesawPosXList.Add(_loadLevelData.seesawPosX2);
        seesawPosXList.Add(_loadLevelData.seesawPosX3);
        seesawPosXList.Add(_loadLevelData.seesawPosX4);

        seesawPosYList.Add(_loadLevelData.seesawPosY1);
        seesawPosYList.Add(_loadLevelData.seesawPosY2);
        seesawPosYList.Add(_loadLevelData.seesawPosY3);
        seesawPosYList.Add(_loadLevelData.seesawPosY4);

        seesawRotateZList.Add(_loadLevelData.seesawRotateZ1);
        seesawRotateZList.Add(_loadLevelData.seesawRotateZ2);
        seesawRotateZList.Add(_loadLevelData.seesawRotateZ3);
        seesawRotateZList.Add(_loadLevelData.seesawRotateZ4);
    }

    void LoadBigWoodPosData()
    {
        bigWoodPosXList = new List<float>();
        bigWoodPosYList = new List<float>();
        bigWoodScaleXList = new List<float>();
        bigWoodScaleYList = new List<float>();
        bigWoodRotateZList = new List<float>();

        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX1);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX2);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX3);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX4);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX5);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX6);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX7);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX8);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX9);
        bigWoodPosXList.Add(_loadLevelData.bigWoodPosX10);

        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY1);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY2);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY3);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY4);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY5);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY6);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY7);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY8);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY9);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY10);

        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX1);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX2);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX3);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX4);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX5);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX6);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX7);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX8);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX9);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX10);

        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY1);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY2);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY3);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY4);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY5);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY6);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY7);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY8);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY9);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY10);

        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ1);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ2);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ3);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ4);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ5);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ6);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ7);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ8);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ9);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ10);
    }
}
