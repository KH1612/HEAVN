using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 3f;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float offsetY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * shakeAmount;
        transform.position = originalPos + new Vector3(offsetX, offsetY, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlayCollect();
            CollectibleManager.instance.AddCollectible(value);
            Destroy(gameObject);
        }
    }
}