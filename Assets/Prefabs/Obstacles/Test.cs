using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
    }
}
