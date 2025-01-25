using UnityEngine;

public class PauseTrigger : MonoBehaviour
{
    public Canvas pauseMenuCanvas; // Reference to your pause menu Canvas
    private bool isPaused = false;

    void Start()
    {
        // Make sure the pause menu is disabled at start
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Check if ESC key was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        // Toggle the pause state
        isPaused = !isPaused;

        // Enable/disable the pause menu Canvas
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.gameObject.SetActive(isPaused);
        }

        // Optionally, you can pause/unpause the game
        Time.timeScale = isPaused ? 0f : 1f;
    }
}