using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OpenSettings() {
        settingsPanel.SetActive(true);
    }

    public void ExitGame() {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
    }
}
