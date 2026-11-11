using UnityEngine;
using UnityEngine.UI;

public class LevelResult : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text title;
    [SerializeField] Button pauseButton;

    public bool IsFinished { get; private set; }

    public void Finish(bool escaped)
    {
        if (IsFinished) return;
        IsFinished = true;
        title.text = escaped ? "you escaped" : "caught";
        pauseButton.interactable = false;
        panel.SetActive(true);
        Time.timeScale = 0f;
    }
}
