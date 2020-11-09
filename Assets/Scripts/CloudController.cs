using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public float boundary;
    public float moveSpeed;


    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < boundary)
            DisableCloud();
    }

    void DisableCloud()
    {
        this.gameObject.SetActive(false);
    }
}
