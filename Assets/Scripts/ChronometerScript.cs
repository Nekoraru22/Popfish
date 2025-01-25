using UnityEngine;
using TMPro;

public class ChronometroScript : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI chronometerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        chronometerText.text = Time.time.ToString("F2");
    }
}
