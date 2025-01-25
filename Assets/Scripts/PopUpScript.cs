using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpScript : MonoBehaviour
{
    public string title;
    public string content;
    public Sprite image;
    [SerializeField] TextMeshProUGUI titleComponent;
    [SerializeField] TextMeshProUGUI contentComponent;
    public Image imageComponent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleComponent.text = title;
        contentComponent.text = content;
        imageComponent.sprite = image;
    }

    public void ClosePopUp() {
        // Destroy the game object parent of this script is attached to
        Destroy(transform.parent.gameObject);
    }
}
