using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxBlood(float blood)
    {
        slider.maxValue = blood;
        slider.value = blood;
    }

    public void SetBlood(float blood)
    {
        slider.value = blood;
    }
}
