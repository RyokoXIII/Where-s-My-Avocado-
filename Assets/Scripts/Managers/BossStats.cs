using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    #region Global Variables

    public int attack;
    public int currentHP, maxHP;

    int _currentLevel;

    #endregion

    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt("levelID");

        if (_currentLevel > 1)
        {
            for (int i = 0; i < _currentLevel - 1; i++)
            {
                LevelUp();
            }
        }
    }

    void LevelUp()
    {
        // Health upgrade
        maxHP += 50;
        currentHP = maxHP;
        // Damage upgrade
        attack += 10;
    }
}
