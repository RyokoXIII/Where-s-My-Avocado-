using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Global Variables

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
    [SerializeField] GameObject _deadZonePrefab;
    [SerializeField] GameObject _roundLogPrefab, _seeSawPrefab, _woodPrefab;

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

    StarHandler _starHandler;

    #endregion


    private void Start()
    {
        // Initilize
        _starHandler = StarHandler.Instance;

        _path = Application.dataPath + "/Resources/Level_1.json";
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
                case "3":
                    _tutorialHand3.SetActive(true);
                    _lineTut3.SetActive(true);
                    break;
            }
        }
    }
}
