using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    static SoundManager _instance;

    public static SoundManager Instance
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

    public AudioSource musicBackground, selectFX, backFX, collectFX, goalFX, victoryFX, loseFX;
}
