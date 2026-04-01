using UnityEngine;

public class ImageShake : MonoBehaviour
{
    public float shakeAmount = 3f;
    public float shakeSpeed = 3f;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        float offsetY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * shakeAmount;
        transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
    }
}