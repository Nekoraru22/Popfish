using UnityEngine;
using UnityEngine.UI;

public class OxygenScript : MonoBehaviour
{
    public Slider slider;

    public void SetMaxOxygen() {
        slider.maxValue = 100;
        slider.value = 100;
    }

    public void SetOxygen(int oxygen) {
        slider.value = oxygen;
    }
}
