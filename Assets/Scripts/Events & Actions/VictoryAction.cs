using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryAction : MonoBehaviour
{
    #region Global Variables

    [SerializeField] Text _collectedCoinTxt, _stageTitleTxt;
    [SerializeField] PlayerStats _playerStats;
    [SerializeField] GameObject _nextBtnObj, _getTripleBtnObj;
    [SerializeField] GameObject _powerUpContainer;

    [Header("Animator")][Space(10f)]
    [SerializeField] Animator _starLabelAnim;
    [SerializeField] Animator _heroUiAnim, _coinLabelAnim;

    UIManager _uiManager;
    SoundManager _soundManager;

    #endregion

    void Start()
    {
        _uiManager = UIManager.Instance;
        _soundManager = SoundManager.Instance;

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
        _soundManager.selectFX.Play();
        _playerStats.currentExp *= 3;

        // Save exp point
        PlayerPrefs.SetInt("expPoint", _playerStats.currentExp);
    }

    public void OnNextToPowerUp()
    {
        _soundManager.selectFX.Play();

        _starLabelAnim.SetBool("IsNext", true);
        _heroUiAnim.SetBool("IsNext", true);
        _coinLabelAnim.SetBool("IsNext", true);

        _nextBtnObj.SetActive(false);
        _getTripleBtnObj.SetActive(false);

        StartCoroutine(PowerUpContainerPopup());
    }

    IEnumerator PowerUpContainerPopup()
    {
        yield return new WaitForSeconds(0.5f);

        _powerUpContainer.SetActive(true);
        _stageTitleTxt.text = "POWER UP!";
    }

    IEnumerator NextButtonLateCall()
    {
        yield return new WaitForSeconds(3f);
        _nextBtnObj.SetActive(true);
    }
}
