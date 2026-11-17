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
            SaveData data = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
            if (data.unlockedLevels == null) data.unlockedLevels = new System.Collections.Generic.List<int>();
            if (!data.unlockedLevels.Contains(1)) data.unlockedLevels.Add(1);
            return data;
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

    public static bool Delete()
    {
        try
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
            string tmp = Path.Combine(Application.persistentDataPath, "save.tmp");
            if (File.Exists(tmp)) File.Delete(tmp);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Could not reset progress: " + e.Message);
            return false;
        }
    }
}
