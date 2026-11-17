using System.IO;
using UnityEngine;

public static class SaveSystem
{
    static string FilePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static SaveData Load()
    {
        if (!File.Exists(FilePath)) return new SaveData();

        try
        {
            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Broken save: " + e.Message);
            return new SaveData();
        }
    }

    public static void Save(SaveData data)
    {
        try
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            string tmp = Path.Combine(Application.persistentDataPath, "save.tmp");
            File.WriteAllText(tmp, JsonUtility.ToJson(data, true));
            if (File.Exists(FilePath)) File.Replace(tmp, FilePath, null);
            else File.Move(tmp, FilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Could not save progress: " + e.Message);
        }
    }
}
