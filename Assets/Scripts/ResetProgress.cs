using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetProgress : MonoBehaviour
{
    [SerializeField] GameObject confirmPanel;
    [SerializeField] SaveManager saveManager;
    [SerializeField] Text message;
    [SerializeField] Button cancelButton;
    [SerializeField] Button resetButton;

    public void AskReset()
    {
        message.text = "escapes: " + saveManager.Data.successfulEscapes +
            "\nbest hearts: " + saveManager.Data.bestHearts +
            "\nopened levels and settings will also be reset.\nlevel 1 will stay available.";
        confirmPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(cancelButton.gameObject);
    }

    public void Cancel()
    {
        confirmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resetButton.gameObject);
    }

    public void ConfirmReset()
    {
        if (!SaveSystem.Delete())
        {
            message.text = "could not reset progress. try again.";
            return;
        }
        PlayerPrefs.DeleteKey("guardLabels");
        PlayerPrefs.DeleteKey("touchControls");
        PlayerPrefs.Save();
        saveManager.ResetToDefaults();
        GameSession.levelIndex = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
