using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] Text progress;

    public SaveData Data { get; private set; }

    void Awake()
    {
        Data = SaveSystem.Load();
        RefreshProgress();
    }

    public void LevelCompleted(int levelIndex, int hearts, int levelCount)
    {
        Data.successfulEscapes++;
        Data.bestHearts = Mathf.Max(Data.bestHearts, hearts);
        int nextLevel = levelIndex + 2;
        if (nextLevel <= levelCount && !Data.unlockedLevels.Contains(nextLevel))
            Data.unlockedLevels.Add(nextLevel);
        SaveSystem.Save(Data);
        RefreshProgress();
    }

    public void ResetToDefaults()
    {
        Data = new SaveData();
        RefreshProgress();
    }

    void RefreshProgress()
    {
        if (progress != null)
            progress.text = "escapes: " + Data.successfulEscapes + "\nbest hearts: " + Data.bestHearts;
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) SaveSystem.Save(Data);
    }
}
