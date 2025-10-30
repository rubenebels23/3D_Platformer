using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{   

    public Slider slider;

    public void SetMaxStamina(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetStamina(float health)
    { 
        slider.value = health;  
    }
}
