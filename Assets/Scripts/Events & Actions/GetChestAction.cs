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

    string _currentAnimation;

    #endregion

    void Start()
    {
        StartCoroutine(StartArmorAnimation());
    }

    IEnumerator StartArmorAnimation()
    {
        SetCharacterState("idle");

        yield return new WaitForSeconds(1f);

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

        SetCharacterState("idle");

        yield return new WaitForSeconds(1f);

        SetCharacterState("open phase1");
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(_currentAnimation))
            return;

        _armorSkeletonGraphic.AnimationState.AddAnimation(0, animation.name, loop, 0).TimeScale = timeScale;

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
        }else if(state == "finisher")
        {
            SetAnimation(_finished, false, 1f);
        }
    }

    public void SetChestState(string state)
    {
        if (state == "idle")
        {
            SetAnimation(_armorIdle, true, 1f);
        }
        else if (state == "fall down")
        {
            SetAnimation(_fallDown, false, 1f);
        }
        else if (state == "open phase1")
        {
            SetAnimation(_open, false, 1f);
        }
    }
}
