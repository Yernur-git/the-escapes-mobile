using UnityEngine;
using UnityEngine.UI;

public class LevelResult : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text title;
    [SerializeField] Button pauseButton;
    [SerializeField] SaveManager saveManager;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] LevelLoader loader;

    public bool IsFinished { get; private set; }

    public void Finish(bool escaped)
    {
        if (IsFinished) return;
        IsFinished = true;
        if (escaped) saveManager.LevelCompleted(GameSession.levelIndex, playerHealth.CurrentHealth, loader.LevelCount);
        title.text = escaped ? "you escaped" : "caught";
        pauseButton.interactable = false;
        panel.SetActive(true);
        Time.timeScale = 0f;
    }
}
