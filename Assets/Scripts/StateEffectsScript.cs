using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateEffectsScript : MonoBehaviour
{
    // Get children of Empty GameObject
    public GameObject debuff;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        disableDebuff();
    }

    public void enableDebuff() {
        debuff.SetActive(true);
    }

    public void disableDebuff() {
        debuff.SetActive(false);
    }
}
