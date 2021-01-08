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
        nextLevelExp = 150;

        UpdatePlayerStats();
    }

    void UpdatePlayerStats()
    {
        if (PlayerPrefs.GetInt("damageStats") == 0)
        {
            attack = 100;
            maxHP = 100;
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
        //if (currentExp >= nextLevelExp && characterLevel < characterMaxLevel)
        //{
        LevelUp();
        //}
    }

    void LevelUp()
    {
        // Minus exp point after upgrading
        currentExp -= nextLevelExp;
        characterLevel++;

        // Health upgrade
        maxHP += 15;
        currentHP = maxHP;
        // Damage upgrade
        attack += 10;
    }
}
