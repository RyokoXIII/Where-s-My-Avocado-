using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    #region Global Variables

    [SerializeField] Camera main;
    [SerializeField] Transform _containerPos;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] Text _coinText;

    [SerializeField] GameObject _coinObj;

    #endregion

    private void Start()
    {
        _coinText.text = _playerManager._expPoint.ToString();
    }

    private void Update()
    {
        if (gameObject.GetComponentInParent<BossManager>())
        {
            _coinText.text = _playerManager._expPoint.ToString();
        }
    }

    private void LateUpdate()
    {
        _coinObj.transform.position = main.WorldToScreenPoint(_containerPos.position);
    }
}
