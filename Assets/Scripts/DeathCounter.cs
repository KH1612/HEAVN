using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class DeathCounter : MonoBehaviour
{
    public static DeathCounter instance;
    public TextMeshProUGUI deathText;
    private int deaths = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateText();
    }

    public void AddDeath()
    {
        deaths++;
        UpdateText();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Destroy(gameObject);
            instance = null;
        }
    }

    void UpdateText()
    {
        if (deathText != null)
            deathText.text = ": " + deaths;
    }
}