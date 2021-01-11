using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [Header("Character Level")]
    public int characterLevel;
    public int characterMaxLevel;

    [Header("EXP")]
    [Space(10f)]
    public int currentExp;
    public int nextLevelExp;

    [Header("Character Stats")]
    [Space(10f)]
    public int attack;
    public int currentHP, maxHP;

    private void Start()
    {
        characterLevel = PlayerPrefs.GetInt("playerLv");

        if (characterLevel > 1)
        {
            nextLevelExp = 300 * characterLevel;
        }
        else
        {
            nextLevelExp = 300;
        }

        currentExp = PlayerPrefs.GetInt("expPoint");

        UpdatePlayerStats();
    }

    void UpdatePlayerStats()
    {
        if (PlayerPrefs.GetInt("damageStats") == 0)
        {
            attack = 100;
            maxHP = 500;
            characterLevel = 1;
        }
        else
        {
            attack = PlayerPrefs.GetInt("damageStats");
            maxHP = PlayerPrefs.GetInt("healthStats");
            characterLevel = PlayerPrefs.GetInt("playerLv");

            currentHP = maxHP;
        }
    }

    public void AddExp()
    {
        LevelUp();
    }

    void LevelUp()
    {
        // Minus exp point after upgrading
        currentExp -= nextLevelExp;
        characterLevel++;

        // Damage upgrade
        attack += 50;
        // Health upgrade
        maxHP += 250;
        currentHP = maxHP;
    }
}
