using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxBlood(float maxBlood)
    {
        slider.maxValue = maxBlood;
    }

    public void SetBlood(float blood)
    {
        slider.value = blood;
    }
}
