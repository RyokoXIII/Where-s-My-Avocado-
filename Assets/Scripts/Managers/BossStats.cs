using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    #region Global Variables

    public int baseAttack = 100;
    public int currentHealth = 250, baseHealth = 250;

    [Header("Stats increase per level")]
    [Space(10f)]
    [SerializeField] int attackPlus = 20;
    [SerializeField] int maxHealthPlus = 50;

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
        else
        {
            baseAttack = 100;

            baseHealth = 250;
            currentHealth = baseHealth;
        }
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
