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
    [SerializeField] int baseNextLevelExp = 100;
    public int currentExp;
    public int nextLevelExp = 100;

    [Header("Character Stats")]
    [Space(10f)]
    public int baseAttack = 100;
    public int currentHealth = 400, baseHealth = 400;

    [Header("Stats increase per Upgrading")]
    [Space(10f)]
    [SerializeField] int attackPlus = 50;
    [SerializeField] int healthPlus = 200;

    private void Start()
    {
        currentExp = PlayerPrefs.GetInt("expPoint");
        characterLevel = PlayerPrefs.GetInt("playerLv");

        if (characterLevel > 1)
        {
            //nextLevelExp = baseNextLevelExp * characterLevel;
            nextLevelExp = (int)(baseNextLevelExp * (0.3f * characterLevel * characterLevel + characterLevel + 1));
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
            baseAttack = 100;
            baseHealth = 400;
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
        //nextLevelExp = baseNextLevelExp * characterLevel;
        nextLevelExp = (int)(baseNextLevelExp * (0.3f * characterLevel * characterLevel + characterLevel + 1));

        // Damage upgrade
        baseAttack += attackPlus;
        // Health upgrade
        baseHealth += healthPlus;
        currentHealth = baseHealth;
    }
}
