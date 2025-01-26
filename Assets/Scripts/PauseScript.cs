using UnityEngine;
using UnityEngine.UIElements;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;

    public void ContinueGame() {
        pauseMenu.SetActive(false);
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
}
