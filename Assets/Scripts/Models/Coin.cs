using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    #region Global Variables

    [SerializeField] Camera main;
    [SerializeField] Transform _containerPos;
    [SerializeField] BossManager _bossManager;
    [SerializeField] EnemyManager _enemyManager;
    [SerializeField] Text _coinText;

    [SerializeField] GameObject _coinObj;

    bool isDead;

    #endregion

    private void Start()
    {
        if (_enemyManager != null)
            _coinText.text = _enemyManager.enemyTotalCoin.ToString();
    }

    private void Update()
    {
        if (_bossManager != null)
        {
            if (_bossManager.currentHealth == 0 && isDead == false)
            {
                _coinText.text = _bossManager.bossTotalCoin.ToString();
                isDead = true;
            }
        }
    }

    private void LateUpdate()
    {
        _coinObj.transform.position = main.WorldToScreenPoint(_containerPos.position);
    }
}
