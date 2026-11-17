using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] Button[] buttons;
    [SerializeField] SaveManager saveManager;
    [SerializeField] Button playButton;

    void OnEnable()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            bool unlocked = saveManager.Data.unlockedLevels.Contains(i + 1);
            buttons[i].interactable = unlocked;
            buttons[i].GetComponentInChildren<Text>().text = levels[i].levelName + "\n" +
                (unlocked ? levels[i].timeLimit + " sec" : "locked - finish level " + i);
        }
        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    public void PlayLevel(int index)
    {
        if (index < 0 || index >= levels.Length || !saveManager.Data.unlockedLevels.Contains(index + 1)) return;
        GameSession.levelIndex = index;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Prison");
    }

    public void Close()
    {
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }
}
