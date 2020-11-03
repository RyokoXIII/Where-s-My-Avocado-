using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    #region Singleton

    static Music _instance;

    public static Music Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UiManager does not exist!");

            return _instance;
        }
    }

    // Initialize instance to this class
    private void Awake()
    {
        _instance = this;
    }

    #endregion


    public void ToggleSound()
    {
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
    }
}
