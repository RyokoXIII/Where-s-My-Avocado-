using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [SerializeField]
    Camera _camera;
    public SpriteRenderer zoom;


    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = zoom.bounds.size.x / zoom.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            _camera.orthographicSize = zoom.bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            _camera.orthographicSize = zoom.bounds.size.y / 2 * differenceInSize;
        }
    }
}
