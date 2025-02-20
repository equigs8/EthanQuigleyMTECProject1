using UnityEngine;
using UnityEngine.UI;

public class CastleHealthBar : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
       
    }
    public void SetHealth(float healthAmount){
        slider.value = healthAmount;
    }
    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
}
