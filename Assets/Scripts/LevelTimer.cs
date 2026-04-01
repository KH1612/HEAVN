using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public static LevelTimer instance;
    public TextMeshProUGUI timerText;
    private float timer;
    private bool isRunning = true;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (timerText == null)
            timerText = GetComponent<TextMeshProUGUI>();
    }
    public float GetTime()
    {
        return timer;
    }
    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 100f) % 100f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0f;
        isRunning = true;
    }
}