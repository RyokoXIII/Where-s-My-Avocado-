using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarHandler : MonoBehaviour
{
    #region Singleton

    static StarHandler _instance;
    public static StarHandler Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("StarHandler does not exist!");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion

    #region Global Variables

    public int levelIndex;
    [HideInInspector]
    public int currentStarNum;
    public GameObject[] starScores;

    #endregion

    private void Start()
    {
        levelIndex = PlayerPrefs.GetInt("levelID");
    }


    public void StarAchieved(int starNumber)
    {
        currentStarNum = starNumber;

        switch (currentStarNum)
        {
            case 1:
                starScores[0].SetActive(true);
                break;
            case 2:
                starScores[0].SetActive(true);
                starScores[1].SetActive(true);
                break;
            case 3:
                starScores[0].SetActive(true);
                starScores[1].SetActive(true);
                starScores[2].SetActive(true);
                break;
            default:
                Debug.Log("No star collected!");
                break;
        }
    }
}
