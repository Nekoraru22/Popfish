using UnityEngine;
using UnityEngine.UI;

public class OxygenScript : MonoBehaviour
{
    public Slider slider;

    public void SetMaxOxygen(float max) {
        slider.maxValue = max;
        slider.value = max;
    }

    public void SetOxygen(float oxygen) {
        slider.value = oxygen;
    }
}
