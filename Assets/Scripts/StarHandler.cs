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
    public Image[] starScores;
    [SerializeField] Sprite _star;

    #endregion


    public void StarAchieved(int starNumber)
    {
        currentStarNum = starNumber;

        switch (currentStarNum)
        {
            case 1:
                starScores[0].sprite = _star;
                break;
            case 2:
                starScores[0].sprite = _star;
                starScores[1].sprite = _star;
                break;
            case 3:
                starScores[0].sprite = _star;
                starScores[1].sprite = _star;
                starScores[2].sprite = _star;
                break;
            default:
                Debug.Log("No star collected!");
                break;
        }
    }
}
