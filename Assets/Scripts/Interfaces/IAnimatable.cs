using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public interface IAnimatable
{
    void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale);

    void SetCharacterState(string state);
}
