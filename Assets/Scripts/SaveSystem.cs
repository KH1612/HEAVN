using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    void Awake()
    {
        instance = this;
    }

    public void SaveLevel(int level, float time, int stars)
    {
        // Save if better than prev value 
        if (!LevelPlayed(level) || time < GetTime(level))
            PlayerPrefs.SetFloat("Level" + level + "Time", time);

        if (stars > GetStars(level))
            PlayerPrefs.SetInt("Level" + level + "Stars", stars);

        PlayerPrefs.Save();
    }

    public float GetTime(int level) => PlayerPrefs.GetFloat("Level" + level + "Time", 0f);
    public int GetStars(int level) => PlayerPrefs.GetInt("Level" + level + "Stars", 0);
    public bool LevelPlayed(int level) => PlayerPrefs.HasKey("Level" + level + "Time");
}