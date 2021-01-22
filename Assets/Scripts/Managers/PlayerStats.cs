using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Character Level")]
    public int characterLevel;
    public int characterMaxLevel;

    [Header("EXP")]
    [Space(10f)]
    [SerializeField] int baseNextLevelExp = 300;
    public int currentExp;
    public int nextLevelExp;

    [Header("Character Stats")]
    [Space(10f)]
    public int baseAttack;
    public int currentHealth, baseHealth;

    [Header("Stats increase per Upgrading")]
    [Space(10f)]
    [SerializeField] int attackPlus = 50;
    [SerializeField] int healthPlus = 250;

    private void Start()
    {
        currentExp = PlayerPrefs.GetInt("expPoint");
        characterLevel = PlayerPrefs.GetInt("playerLv");

        if (characterLevel > 1)
        {
            nextLevelExp = baseNextLevelExp * characterLevel;
        }
        else
        {
            characterLevel = 1;
            nextLevelExp = baseNextLevelExp;
        }
        UpdatePlayerStats();
    }

    void UpdatePlayerStats()
    {
        if (PlayerPrefs.GetInt("damageStats") == 0)
        {
            //attack = 100;
            //maxHP = 500;
            characterLevel = 1;
        }
        else
        {
            baseAttack = PlayerPrefs.GetInt("damageStats");
            baseHealth = PlayerPrefs.GetInt("healthStats");
            characterLevel = PlayerPrefs.GetInt("playerLv");

        }
        currentHealth = baseHealth;
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
        nextLevelExp = baseNextLevelExp * characterLevel;

        // Damage upgrade
        baseAttack += attackPlus;
        // Health upgrade
        baseHealth += healthPlus;
        currentHealth = baseHealth;
    }
}
