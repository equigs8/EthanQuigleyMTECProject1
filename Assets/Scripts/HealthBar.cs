using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float healthAmount){
        slider.value = healthAmount;
    }
    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
}
