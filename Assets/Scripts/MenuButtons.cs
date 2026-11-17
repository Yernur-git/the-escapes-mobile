using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void Play()
    {
        PlayLevel(0);
    }

    public void PlayLevel(int index)
    {
        GameSession.levelIndex = index;
        SceneManager.LoadScene("Prison");
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
