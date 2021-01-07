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
    public int nextLevelExp, expPoint;

    [Header("Character Stats")]
    [Space(10f)]
    public int currentHP;
    public int maxHP, attack;

    private void Start()
    {
        nextLevelExp = 150;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddExp();
        }
    }

    public void AddExp()
    {
        currentExp += expPoint;

        if (currentExp >= nextLevelExp && characterLevel < characterMaxLevel)
        {
            LevelUp();
        }
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
