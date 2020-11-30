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

    PoolManager _pooler;
    SoundManager _soundManager;

    #endregion

    private void Start()
    {
        _pooler = PoolManager.Instance;
        _soundManager = SoundManager.Instance;

        levelIndex = PlayerPrefs.GetInt("levelID");
    }


    public void StarAchieved(int starNumber)
    {
        currentStarNum = starNumber;

        StartCoroutine(ShowStarLateCall());
    }

    IEnumerator ShowStarLateCall()
    {
        switch (currentStarNum)
        {
            case 1:
                yield return new WaitForSeconds(0.5f);
                _pooler.SpawnFromPool("Star Particle", starScores[0].transform.position, Quaternion.identity);
                _soundManager.showStarFX.Play();
                starScores[0].SetActive(true);

                break;
            case 2:
                yield return new WaitForSeconds(0.5f);
                _pooler.SpawnFromPool("Star Particle", starScores[0].transform.position, Quaternion.identity);
                _soundManager.showStarFX.Play();
                starScores[0].SetActive(true);

                yield return new WaitForSeconds(0.5f);
                _pooler.SpawnFromPool("Star Particle", starScores[1].transform.position, Quaternion.identity);
                _soundManager.showStarFX.Play();
                starScores[1].SetActive(true);

                break;
            case 3:
                yield return new WaitForSeconds(0.5f);
                _pooler.SpawnFromPool("Star Particle", starScores[0].transform.position, Quaternion.identity);
                _soundManager.showStarFX.Play();
                starScores[0].SetActive(true);

                yield return new WaitForSeconds(0.5f);
                _pooler.SpawnFromPool("Star Particle", starScores[1].transform.position, Quaternion.identity);
                _soundManager.showStarFX.Play();
                starScores[1].SetActive(true);

                yield return new WaitForSeconds(0.5f);
                _pooler.SpawnFromPool("Star Particle", starScores[2].transform.position, Quaternion.identity);
                _soundManager.showStarFX.Play();
                starScores[2].SetActive(true);

                break;
            default:
                Debug.Log("No star collected!");
                break;
        }
    }
}
