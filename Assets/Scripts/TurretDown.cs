using UnityEngine;

public class TurretDown : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 0.1f;
    public float bulletSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float frameTimer;
    private int direction = 1; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= frameRate)
        {
            frameTimer = 0f;
            AdvanceFrame();
        }
    }

    void AdvanceFrame()
    {
        currentFrame += direction;

        if (currentFrame >= frames.Length - 1)
        {
            currentFrame = frames.Length - 1;
            direction = -1;
            Shoot(Vector2.down);
        }
        else if (currentFrame <= 0)
        {
            currentFrame = 0;
            direction = 1;
            Shoot(Vector2.down);
        }

        spriteRenderer.sprite = frames[currentFrame];
    }

    void Shoot(Vector2 dir)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(dir);
    }
}