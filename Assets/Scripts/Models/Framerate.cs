using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Framerate : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}
