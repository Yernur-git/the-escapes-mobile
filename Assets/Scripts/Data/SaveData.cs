using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int version = 1;
    public int successfulEscapes;
    public int bestHearts;
    public List<int> unlockedLevels = new() { 1 };
}
