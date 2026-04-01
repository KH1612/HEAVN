using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public int nextSceneIndex = 2;
    public int levelIndex = 1;
    public GameObject levelFinishedUI;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 3f;
    public int collectiblesForStar = 10;
    public float timeForStar = 60f;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    private Vector3 originalPos;

    void Start()
    {
        levelFinishedUI.SetActive(false);
        originalPos = transform.localPosition;
        if (star1) star1.SetActive(false);
        if (star2) star2.SetActive(false);
        if (star3) star3.SetActive(false);
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float offsetY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * shakeAmount;
        transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlayLevelEnd();
            Debug.Log("Level end triggered");
            levelFinishedUI.SetActive(true);
            Time.timeScale = 0f;
            CalculateStars();
        }
    }

    void CalculateStars()
    {
        int stars = 0;

        if (star1) star1.SetActive(true);
        stars++;

        if (CollectibleManager.instance.GetCount() >= collectiblesForStar)
        {
            if (star2) star2.SetActive(true);
            stars++;
        }

        if (LevelTimer.instance.GetTime() <= timeForStar)
        {
            if (star3) star3.SetActive(true);
            stars++;
        }

        Debug.Log("Stars calculated: " + stars);
        Debug.Log("SaveSystem instance: " + SaveSystem.instance);
        SaveSystem.instance.SaveLevel(levelIndex, LevelTimer.instance.GetTime(), stars);
    }

    public void NextLevel()
    {
        Debug.Log("Scene " + nextSceneIndex + " Clicked.");
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void MainMenu()
    {
        Debug.Log("Main menu clicked.");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}