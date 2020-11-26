using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAnimationController : MonoBehaviour
{
    [SerializeField]
    Animator _slash1Anim, _slash2Anim;


    void Start()
    {
        AnimationLateCall();
    }

    void AnimationLateCall()
    {
        _slash1Anim.Play("Slash");

        _slash2Anim.Play("Slash_2");
    }
}
