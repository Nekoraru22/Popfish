using UnityEngine;

public class EndLoreScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
