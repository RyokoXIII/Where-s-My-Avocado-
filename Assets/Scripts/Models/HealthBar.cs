using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Camera main;
    [SerializeField] Transform targetPos;

    public Slider slider;
    public Vector3 wantedPos;
    
    void LateUpdate()
    {
        slider.transform.position = main.WorldToScreenPoint(targetPos.position + wantedPos);
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
