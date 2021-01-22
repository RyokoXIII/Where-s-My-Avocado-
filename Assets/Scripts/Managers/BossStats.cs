using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    #region Global Variables

    public int baseAttack;
    public int currentHealth, baseHealth;

    [Header("Stats increase per level")]
    [Space(10f)]
    [SerializeField] int attackPlus = 15;
    [SerializeField] int maxHealthPlus = 75;

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
        //else
        //{
        //    maxHP = 500;
        //    currentHP = maxHP;

        //    attack = 100;
        //}
    }

    void LevelUp()
    {
        // Health upgrade
        baseHealth += maxHealthPlus;
        currentHealth = baseHealth;
        // Damage upgrade
        baseAttack += attackPlus;
    }
}
