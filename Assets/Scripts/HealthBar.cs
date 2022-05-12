using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    //GameObject slider;
    Slider slider;

    public void InitHealthBar(int maxHealth, int currentHealth)
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxHealth;
        slider.value = 0; // hide bar if no damage is taken yet //currentHealth;
    }

    public void UpdateBar(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
