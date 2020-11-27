using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    public GameObject onImg, offImg;
    SoundManager _soundManager;
    UIManager _uiManager;

    public bool firstMusicSourceIsPlaying;

    [Space(10f)]
    [SerializeField] GameObject _beachMusicBackground;
    [SerializeField] GameObject _titleMusic;


    private void Start()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

        _uiManager.OnOption += MusicButton;
        UpdateButtonState();
    }

    public void MusicButton()
    {
        _soundManager.selectFX.Play();

        if (PlayerPrefs.GetInt("muted", 0) == 0)
        {
            PlayerPrefs.SetInt("muted", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("muted", 0);
            PlayerPrefs.Save();
        }
        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? _soundManager.titleMusic : _soundManager.beachMusicBackground;

        if (PlayerPrefs.GetInt("muted", 0) == 0)
        {
            AudioListener.volume = 1;

            //activeSource.volume = 0.4f;
            //activeSource.Play();
            if (_beachMusicBackground.activeInHierarchy == true)
            {
                _soundManager.beachMusicBackground.Play();
            }

            onImg.SetActive(true);
            offImg.SetActive(false);
        }
        else
        {
            //activeSource.volume = 0;

            AudioListener.volume = 0;

            onImg.SetActive(false);
            offImg.SetActive(true);
        }
    }
}
