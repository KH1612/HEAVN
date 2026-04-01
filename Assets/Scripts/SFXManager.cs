using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip jumpSound;
    public AudioClip collectSound;
    public AudioClip levelEndSound;
    public AudioClip deathSound;
    public AudioClip boostSound;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump() => audioSource.PlayOneShot(jumpSound, 2f);
    public void PlayCollect() => audioSource.PlayOneShot(collectSound, 1.5f);
    public void PlayLevelEnd() => audioSource.PlayOneShot(levelEndSound, 1.5f);
    public void PlayDeath() => audioSource.PlayOneShot(deathSound, 2f);
    public void PlayBoost() => audioSource.PlayOneShot(boostSound, 1.5f);
}
