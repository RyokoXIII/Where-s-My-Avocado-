using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryAction : MonoBehaviour
{
    #region Global Variables

    [SerializeField] Text _collectedCoinTxt;
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] GameObject _nextBtnObj;

    UIManager _uiManager;

    #endregion

    void Start()
    {
        _uiManager = UIManager.Instance;

        _uiManager.OnGetTriple += OnGetTriple;
        _uiManager.OnNextToPowerUp += OnNextToPowerUp;

        // Show next button
        StartCoroutine(NextButtonLateCall());
    }

    void Update()
    {
        // Collected coins
        _collectedCoinTxt.text = _playerStats.currentExp.ToString();
    }

    public void OnGetTriple()
    {
        _playerStats.currentExp *= 3;

        // Save exp point
        PlayerPrefs.SetInt("expPoint", _playerStats.currentExp);
    }

    public void OnNextToPowerUp()
    {

    }

    IEnumerator NextButtonLateCall()
    {
        yield return new WaitForSeconds(3f);
        _nextBtnObj.SetActive(true);
    }
}
