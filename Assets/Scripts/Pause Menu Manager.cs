using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseMenu; 

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !GameManager.Instance.matchOver)
        {
            if(isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void QuitToBackMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
