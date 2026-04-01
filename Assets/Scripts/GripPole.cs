using UnityEngine;

public class GripPole : MonoBehaviour
{
    public float launchForce = 15f;

    public void ExecuteLaunch(Rigidbody2D playerRb) {
        Vector2 dir = (transform.position - playerRb.transform.position).normalized;
        playerRb.linearVelocity = Vector2.zero;
        //playerRb.AddForce(dir * launchForce, ForceMode2D.Impulse);
        playerRb.linearVelocity = dir * launchForce;
        Debug.Log($"Launching in direction: {dir}");
    }
}