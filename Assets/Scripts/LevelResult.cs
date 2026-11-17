using UnityEngine;
using UnityEngine.UI;

public class LevelResult : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text title;
    [SerializeField] Button pauseButton;
    [SerializeField] SaveManager saveManager;
    [SerializeField] PlayerHealth playerHealth;

    public bool IsFinished { get; private set; }

    public void Finish(bool escaped)
    {
        if (IsFinished) return;
        IsFinished = true;
        if (escaped) saveManager.RecordEscape(playerHealth.CurrentHealth);
        title.text = escaped ? "you escaped" : "caught";
        pauseButton.interactable = false;
        panel.SetActive(true);
        Time.timeScale = 0f;
    }
}
