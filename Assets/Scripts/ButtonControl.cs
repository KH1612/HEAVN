using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonShake : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float shakeAmount = 3f;
    public float shakeSpeed = 20f;
    private Vector3 originalPos;
    private bool isShaking;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            float offsetY = Mathf.Cos(Time.time * shakeSpeed * 1.3f) * shakeAmount;
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
        }
        else
        {
            transform.localPosition = originalPos;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isShaking = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isShaking = false;
    }
}