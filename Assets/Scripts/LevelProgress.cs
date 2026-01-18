using UnityEngine;

public static class LevelProgress
{
    // 0 = only Street unlocked
    // 1 = Garden unlocked
    // 2 = Home unlocked
    private const string Key = "UnlockedLevelIndex";

    public static int GetUnlockedIndex()
    {
        return PlayerPrefs.GetInt(Key, 0);
    }

    public static void UnlockIndex(int index)
    {
        int current = GetUnlockedIndex();
        if (index > current)
        {
            PlayerPrefs.SetInt(Key, index);
            PlayerPrefs.Save();
        }
    }

    public static bool IsUnlocked(int index)
    {
        return GetUnlockedIndex() >= index;
    }

    // Optional: for testing
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save();
    }
}
