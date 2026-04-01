using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        // Load saved volume, default to 1 if never set
        slider.value = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = slider.value;
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}