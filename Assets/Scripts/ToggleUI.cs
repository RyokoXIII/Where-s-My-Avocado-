using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    public GameObject onImg, offImg;


    private void Start()
    {
        UpdateButtonState();
    }

    public void ToggleSoundButton()
    {
        SoundManager.Instance.selectFX.Play();

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
        if (PlayerPrefs.GetInt("muted", 0) == 0)
        {
            AudioListener.volume = 1;
            SoundManager.Instance.musicBackground.Play();
            onImg.SetActive(true);
            offImg.SetActive(false);
        }
        else
        {
            onImg.SetActive(false);
            offImg.SetActive(true);
            AudioListener.volume = 0;
        }
    }
}
