using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip music;

    void Start()
    {
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }
}