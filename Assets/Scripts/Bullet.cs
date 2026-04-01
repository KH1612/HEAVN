using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 10f;
    private Vector2 direction;
    private Vector3 originalPos;
    void Start()
    {
        Destroy(gameObject, 15f);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        originalPos = transform.position;
    }

    void Update()
    {
        originalPos += (Vector3)(direction * speed * Time.deltaTime);
        float offsetY = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        transform.position = originalPos + new Vector3(0f, offsetY, 0f);
    }
}