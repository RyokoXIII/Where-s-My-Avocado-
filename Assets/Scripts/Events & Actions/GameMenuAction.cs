using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuAction : MonoBehaviour
{
    UIManager _uiManager;
    SoundManager _soundManager;


    private void Start()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

        _uiManager.OnBack += Resume;
    }

    public void Resume()
    {
        if (gameObject.activeInHierarchy == true)
        {
            _soundManager.backFX.Play();
            gameObject.SetActive(false);

            Time.timeScale = 1f;
        }
    }
}
