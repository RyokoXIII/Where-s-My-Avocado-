using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetChestAction : MonoBehaviour, IAnimatable
{
    #region Global Variables

    // Image & Animation
    [Header("Animation")]
    [Space(10f)]
    [SerializeField] SkeletonGraphic _armorSkeletonGraphic;
    [SerializeField] SkeletonGraphic _chestSkeletonGraphic;
    [SerializeField] AnimationReferenceAsset _armorIdle, _circle, _finished;
    [SerializeField] AnimationReferenceAsset _chestIdle, _fallDown, _open;

    [Header("Game Objects")]
    [Space(10f)]
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] GameObject _armor, _chest;
    [SerializeField] GameObject _chestPanel;
    [SerializeField] GameObject _chestMenu, _continueMenu;
    [SerializeField] Text _armorTxt;

    public bool openChest;

    string _currentAnimation;

    UIManager _uiManager;
    StarHandler _starHandler;

    #endregion

    void Start()
    {
        _starHandler = StarHandler.Instance;
        _uiManager = UIManager.Instance;

        _uiManager.OnGetArmor += OnGetArmor;
        _uiManager.OnLooseIt += OnLooseIt;
        _uiManager.OnContinue += OnContinue;

        StartCoroutine(StartChestAnimation());
    }

    public void OnGetArmor()
    {
        PlayerPrefs.SetString("upgradeHero", "upgraded");
        openChest = true;

        SetChestState("open phase2");
        _chestMenu.SetActive(false);

        StartCoroutine(OpenChest());
        StartCoroutine(StartArmorAnimation());
    }

    public void OnLooseIt()
    {
        PlayerPrefs.SetInt("levelReceivedChest", _starHandler.levelIndex);
        StartCoroutine(ContinueToGameOverMenu());
    }

    public void OnContinue()
    {
        StartCoroutine(ContinueToGameOverMenu());
    }

    IEnumerator StartArmorAnimation()
    {
        SetCharacterState("idle");

        yield return new WaitForSeconds(1f);

        _armorTxt.text = "DARK KNIGHT ARMOR";
        _continueMenu.SetActive(true);

        SetCharacterState("circle");

        yield return new WaitForSeconds(1f);

        SetCharacterState("finisher");

        yield return new WaitForSeconds(1f);

        SetCharacterState("idle");
    }

    IEnumerator StartChestAnimation()
    {
        SetChestState("fall down");

        yield return new WaitForSeconds(1f);

        SetChestState("idle4");
    }

    IEnumerator OpenChest()
    {
        yield return new WaitForSeconds(1f);

        _chest.SetActive(false);
        _armor.SetActive(true);
    }

    IEnumerator ContinueToGameOverMenu()
    {
        _chestPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        _playerManager.GameOverPopup();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
            return;

        _armorSkeletonGraphic.AnimationState.AddAnimation(0, animation.name, loop, 0).TimeScale = timeScale;

        _currentAnimation = animation.name;
    }

    public void SetChestAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
            return;

        _chestSkeletonGraphic.AnimationState.AddAnimation(0, animation.name, loop, 0).TimeScale = timeScale;

        _currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state == "idle")
        {
            SetAnimation(_armorIdle, true, 1f);
        }
        else if (state == "circle")
        {
            SetAnimation(_circle, false, 1f);
        }
        else if (state == "finisher")
        {
            SetAnimation(_finished, false, 1f);
        }
    }

    public void SetChestState(string state)
    {
        if (state == "idle4")
        {
            SetChestAnimation(_chestIdle, true, 1f);
        }
        else if (state == "fall down")
        {
            SetChestAnimation(_fallDown, false, 1f);
        }
        else if (state == "open phase2")
        {
            SetChestAnimation(_open, false, 1f);
        }
    }
}
