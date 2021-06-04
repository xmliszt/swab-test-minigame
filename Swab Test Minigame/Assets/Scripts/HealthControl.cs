using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    public Slider slider;
 
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public int GetHealth()
    {
        return (int) slider.value;
    }
}
