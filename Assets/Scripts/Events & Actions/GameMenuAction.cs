using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuAction : MonoBehaviour
{

    private void Start()
    {
        UIManager.Instance.OnBack += Resume;
    }

    public void Resume()
    {
        if (gameObject.activeInHierarchy == true)
        {
            SoundManager.Instance.backFX.Play();
            gameObject.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}
