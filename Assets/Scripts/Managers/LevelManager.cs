using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] Transform _npcPos;
    [SerializeField] NpcManager _npcManager;


    [Header("Grounds")]
    [Space(10f)]
    [SerializeField] GameObject _beachBackground;
    [SerializeField] GameObject _sunsetBackground, _nightBackground;
    [SerializeField] GameObject _foregroundContainer;
    [SerializeField] GameObject[] levelForegroundList;

    [Header("Items")]
    [Space(10f)]
    [SerializeField] Transform[] starPosList;

    [Header("Obstacles")]
    [Space(10f)]
    [SerializeField] GameObject _obstaclesContainer;
    [SerializeField] LineManager _bigWoodContainer;
    [SerializeField] GameObject _deadZonePrefab;
    [SerializeField] GameObject _roundLogPrefab, _seeSawPrefab, _woodPrefab, _bigWoodPrefab;

    [Header("Musics")]
    [Space(10f)]
    [SerializeField] GameObject _sunsetMusicBackground;
    [SerializeField] GameObject _beachMusicBackground;

    [Header("Tutorial")]
    [Space(10f)]
    [SerializeField] GameObject _tutorial;
    public GameObject _tutorialHand1, _tutorialHand2, _tutorialHand3;
    public GameObject _lineTut1, _lineTut2, _lineTut3;

    LevelData _loadLevelData;
    string _json;
    string _path;

    // List of Obstacles
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
        for (int i = 0; i < dataList.Length; i++)
        {
            if (dataList[i].Equals(PlayerPrefs.GetInt("levelID")))
            {
                _path = Application.dataPath + "/Resources/" + dataList[i].ToString() + ".json";
            }
        }

        LoadLevelData(_path);
    }

    void LoadLevelData(string path)
    {
        _json = File.ReadAllText(path);
        Debug.Log(_json);
        // Get all data from json
        _loadLevelData = JsonUtility.FromJson<LevelData>(_json);

        // Load Level data
        LoadCharacterData();
        LoadBackgroundData();
        LoadForegroundData();
        LoadItemsData();
        LoadObstacles();
        LoadBackgroundMusic();
        LoadTutorial();

    }

    void LoadCharacterData()
    {
        // Player pos
        Vector3 newPlayerPos = new Vector3(_loadLevelData.playerPosX, _loadLevelData.playerPosY, 0f);
        _playerPos.transform.position = newPlayerPos;

        // Npc pos
        Vector3 newNpcPos = new Vector3(_loadLevelData.npcPosX, _loadLevelData.npcPosY, 0f);
        _npcPos.transform.position = newNpcPos;

        // Npc particle pos
        _npcManager.xPos = _loadLevelData.npcParticlePosX;
        _npcManager.yPos = _loadLevelData.npcParticlePosY;
    }

    void LoadBackgroundData()
    {
        if (_loadLevelData.beach_background == true)
        {
            _beachBackground.SetActive(true);
        }
        else
        {
            _sunsetBackground.SetActive(true);
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

    void LoadItemsData()
    {
        Vector3 newStarPos_1 = new Vector3(_loadLevelData.starPosX_1, _loadLevelData.starPosY_1, 0f);
        Vector3 newStarPos_2 = new Vector3(_loadLevelData.starPosX_2, _loadLevelData.starPosY_2, 0f);
        Vector3 newStarPos_3 = new Vector3(_loadLevelData.starPosX_3, _loadLevelData.starPosY_3, 0f);

        // Assign new pos to each stars
        starPosList[0].transform.position = newStarPos_1;
        starPosList[1].transform.position = newStarPos_2;
        starPosList[2].transform.position = newStarPos_3;
    }

    void LoadBackgroundMusic()
    {
        if (_loadLevelData.sunset_music == true)
        {
            _beachMusicBackground.SetActive(false);
        }
        else
        {
            _sunsetMusicBackground.SetActive(true);
        }
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
            Vector2 newPos = new Vector2(_loadLevelData.roundLogPosX, _loadLevelData.roundLogPosY);
            Vector3 newScale = new Vector3(_loadLevelData.roundLogScaleX, _loadLevelData.roundLogScaleY, _loadLevelData.roundLogScaleZ);

            GameObject obj = Instantiate(_roundLogPrefab, newPos, Quaternion.identity);
            obj.transform.parent = _obstaclesContainer.transform;
            obj.transform.localScale = newScale;
        }

        if (_loadLevelData.wood == true)
        {
            LoadWoodData();

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
            LoadSeesawData();

            for (int i = 0; i < _loadLevelData.seesawNum; i++)
            {
                Vector2 newPos = new Vector2(seesawPosXList[i], seesawPosYList[i]);
                Quaternion newRotate = Quaternion.Euler(0, 0, seesawRotateZList[i]);

                GameObject obj = Instantiate(_seeSawPrefab, newPos, newRotate);
                obj.transform.parent = _obstaclesContainer.transform;

                Vector3 newScale = new Vector3(_loadLevelData.seesawScaleX, _loadLevelData.seesawScaleY, obj.transform.localScale.z);
                obj.transform.localScale = newScale;
            }
        }

        if (_loadLevelData.bigWood == true)
        {
            LoadBigWoodData();

            for (int i = 0; i < _loadLevelData.bigWoodNum; i++)
            {
                Vector2 newPos = new Vector2(bigWoodPosXList[i], bigWoodPosYList[i]);
                Quaternion newRotate = Quaternion.Euler(0, 0, bigWoodRotateZList[i]);

                GameObject obj = Instantiate(_bigWoodPrefab, newPos, newRotate);

                // Add obj to bigWood list in LineManager
                Rigidbody2D bigWoodRb = obj.GetComponent<Rigidbody2D>();
                _bigWoodContainer.bigWoodRbs.Add(bigWoodRb);

                obj.transform.parent = _obstaclesContainer.transform;

                Vector3 newScale = new Vector3(bigWoodScaleXList[i], bigWoodScaleYList[i], obj.transform.localScale.z);
                obj.transform.localScale = newScale;
            }
        }

        if(_loadLevelData.woodNest == true)
        {
            LoadBigWoodData();

            for (int i = 0; i < _loadLevelData.woodNestNum; i++)
            {
                Vector2 newPos = new Vector2(bigWoodPosXList[i], bigWoodPosYList[i]);
                Quaternion newRotate = Quaternion.Euler(0, 0, bigWoodRotateZList[i]);

                GameObject obj = Instantiate(_bigWoodPrefab, newPos, newRotate);

                // Add obj to smallWood list in LineManager
                Rigidbody2D smallWoodRb = obj.GetComponent<Rigidbody2D>();
                _bigWoodContainer.smallWoodRbs.Add(smallWoodRb);

                obj.transform.parent = _obstaclesContainer.transform;

                Vector3 newScale = new Vector3(bigWoodScaleXList[i], bigWoodScaleYList[i], obj.transform.localScale.z);
                obj.transform.localScale = newScale;
            }
        }
    }

    void LoadWoodData()
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

    void LoadSeesawData()
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

    void LoadBigWoodData()
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

        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY1);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY2);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY3);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY4);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY5);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY6);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY7);
        bigWoodPosYList.Add(_loadLevelData.bigWoodPosY8);

        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX1);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX2);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX3);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX4);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX5);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX6);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX7);
        bigWoodScaleXList.Add(_loadLevelData.bigWoodScaleX8);

        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY1);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY2);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY3);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY4);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY5);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY6);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY7);
        bigWoodScaleYList.Add(_loadLevelData.bigWoodScaleY8);

        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ1);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ2);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ3);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ4);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ5);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ6);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ7);
        bigWoodRotateZList.Add(_loadLevelData.bigWoodRotateZ8);
    }
}
