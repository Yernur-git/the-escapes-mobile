using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    public void SetPaused(bool paused)
    {
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
        MusicManager.Instance.SetPaused(paused);
    }

    public void Resume()
    {
        SetPaused(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        MusicManager.Instance.SetPaused(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        MusicManager.Instance.SetPaused(false);
        SceneManager.LoadScene("MainMenu");
    }
}
