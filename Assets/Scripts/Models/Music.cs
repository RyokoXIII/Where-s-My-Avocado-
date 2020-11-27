using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public void ToggleMusicButton()
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
