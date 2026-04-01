using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public Button level2Button;
    public Button level3Button;
    public ButtonShake level2Shake;
    public ButtonShake level3Shake;
    public MainMenuStats mainMenuStats;
    public GameObject infoUI;
    public GameObject infoButton;
    public float shakeAmount = 3f;
    public float shakeSpeed = 3f;
    private Vector3 infoButtonOriginalPos;

    void Start()
    {
        bool level2Unlocked = SaveSystem.instance.GetStars(1) >= 1;
        bool level3Unlocked = SaveSystem.instance.GetStars(2) >= 1;
        level2Button.interactable = level2Unlocked;
        level3Button.interactable = level3Unlocked;
        level2Shake.enabled = level2Unlocked;
        level3Shake.enabled = level3Unlocked;

        infoUI.SetActive(false);
        if (infoButton != null)
            infoButtonOriginalPos = infoButton.transform.localPosition;
    }

    void Update()
    {
        if (infoButton != null)
        {
            float offsetX = Mathf.Sin((Time.time + 1.5f) * shakeSpeed) * shakeAmount;
            float offsetY = Mathf.Cos((Time.time + 1.5f) * shakeSpeed * 1.3f) * shakeAmount;
            infoButton.transform.localPosition = infoButtonOriginalPos + new Vector3(offsetX, offsetY, 0f);
        }
    }

    public void OpenInfo()
    {
        infoUI.SetActive(true);
    }

    public void CloseInfo()
    {
        infoUI.SetActive(false);
    }

    public void ResetStats()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Start();
        mainMenuStats.Start();
    }
    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Level2()
    {
        if (SaveSystem.instance.GetStars(1) >= 1)
            SceneManager.LoadScene("Level2");
    }
    public void Level3()
    {
        if (SaveSystem.instance.GetStars(2) >= 1)
            SceneManager.LoadScene("Level3");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}