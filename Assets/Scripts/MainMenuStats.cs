using UnityEngine;
using TMPro;

public class MainMenuStats : MonoBehaviour
{
    [Header("Level 1")]
    public GameObject level1Star1;
    public GameObject level1Star2;
    public GameObject level1Star3;
    public TextMeshProUGUI level1Time;

    [Header("Level 2")]
    public GameObject level2Star1;
    public GameObject level2Star2;
    public GameObject level2Star3;
    public TextMeshProUGUI level2Time;

    [Header("Level 3")]
    public GameObject level3Star1;
    public GameObject level3Star2;
    public GameObject level3Star3;
    public TextMeshProUGUI level3Time;

    public void Start()
    {
        DisplayStats(1, level1Star1, level1Star2, level1Star3, level1Time);
        DisplayStats(2, level2Star1, level2Star2, level2Star3, level2Time);
        DisplayStats(3, level3Star1, level3Star2, level3Star3, level3Time);
    }

    void DisplayStats(int level, GameObject s1, GameObject s2, GameObject s3, TextMeshProUGUI timeText)
    {
        s1.SetActive(false);
        s2.SetActive(false);
        s3.SetActive(false);

        if (!SaveSystem.instance.LevelPlayed(level))
        {
            timeText.text = "--:--:--";
            return;
        }

        int stars = SaveSystem.instance.GetStars(level);
        if (stars >= 1) s1.SetActive(true);
        if (stars >= 2) s2.SetActive(true);
        if (stars >= 3) s3.SetActive(true);

        float t = SaveSystem.instance.GetTime(level);
        int mins = Mathf.FloorToInt(t / 60f);
        int secs = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 100f) % 100f);
        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", mins, secs, ms);
    }
}