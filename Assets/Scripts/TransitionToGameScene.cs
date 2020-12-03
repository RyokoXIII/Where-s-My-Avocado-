using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToGameScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        yield return new WaitForSeconds(Random.Range(0.3f,1f));

        int levelSceneIndex;

        if (PlayerPrefs.GetInt("backtomenu") == 1)
        {
            levelSceneIndex = 0;
            PlayerPrefs.SetInt("backtomenu", 0);
        }
        else
        {
            levelSceneIndex = 2;
        }
        SceneManager.LoadScene(levelSceneIndex);
    }
}
