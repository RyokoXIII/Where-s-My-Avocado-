using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuAction : MonoBehaviour
{
    UIManager _uiManager;
    SoundManager _soundManager;
    StarHandler _starHandler;

    [SerializeField] Text _levelName;


    private void Start()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;
        _starHandler = StarHandler.Instance;

        _uiManager.OnBack += Resume;

        _levelName.text = "Level " + _starHandler.levelIndex.ToString();
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
