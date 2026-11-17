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

    public void RecordEscape(int hearts)
    {
        Data.successfulEscapes++;
        Data.bestHearts = Mathf.Max(Data.bestHearts, hearts);
        SaveSystem.Save(Data);
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
