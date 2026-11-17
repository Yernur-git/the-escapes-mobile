using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] LevelSelect levelSelect;

    public void Play()
    {
        levelSelect.gameObject.SetActive(true);
    }

    public void PlayLevel(int index)
    {
        levelSelect.PlayLevel(index);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
