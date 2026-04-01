using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Player : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float jumpHoldForce = 20f;
    public float maxJumpHoldTime = 0.2f;
    public float dashForce = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
    public float apexThreshold = 2f;
    public float apexGravityScale = 0.5f;
    public float defaultGravity = 1f;
    public float wallJumpForceX = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;
    public int extraJumpsVal = 2;
    private int extraJumps;
    private float jumpHoldTimer;
    private bool isJumpHeld;
    private InputAction jumpAction;
    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    [Header("Wall Sliding")]
    public float wallSlideSpeed = 2f;
    public float wallCheckDist = 1f;
    private bool isTouchingWall;
    private bool isWallSliding;
    public UnityEngine.UI.Image blackScreen;
    [Header("Grappling")]
    private GripPole currentGripPole;
    private bool isGrappling = false;
    private float grappleTimer = 0f;
    public float grappleDuration = 0.25f;
    public float wallJumpTime = 0.2f; // How long input is disabled after jumping
    private float wallJumpCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        extraJumps = extraJumpsVal;
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        blackScreen.color = new Color(1, 1, 1, 0);
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isWallSliding)
            {
                SoundManager.instance.PlayJump();
                // Check which side the wall is on to jump in the opposite direction
                float wallDir = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDist, groundLayer) ? 1f : -1f;
                
                // Jump AWAY from the wall (negative wallDir)
                rb.linearVelocity = new Vector2(-wallDir * wallJumpForceX, jumpForce);
                
                wallJumpCounter = wallJumpTime; // Set the timer to block movement input
                isJumpHeld = true;
                jumpHoldTimer = 0f;
                animator.SetTrigger("Jump");
            }
            else if (isGrounded)
            {
                SoundManager.instance.PlayJump();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps = extraJumpsVal;
                isJumpHeld = true;
                jumpHoldTimer = 0f;
                animator.SetTrigger("Jump");
            }
            else if (extraJumps > 0)
            {
                SoundManager.instance.PlayJump();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
                isJumpHeld = true;
                jumpHoldTimer = 0f;
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            isJumpHeld = false;
        }
    }
    void OnDash()
    {
        if (dashCooldownTimer <= 0f && !isDashing)
        {
            SoundManager.instance.PlayBoost();
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            float dashDir = moveInput.x != 0 ? Mathf.Sign(moveInput.x) : Mathf.Sign(transform.localScale.x);
            Debug.Log("Dash direction: " + dashDir);
            rb.linearVelocity = new Vector2(dashDir * dashForce, 0f);
            rb.gravityScale = 0f;
            animator.SetTrigger("Dash");
        }
    }

    void OnGrapple(InputValue value)
    {
        if (value.isPressed && currentGripPole != null)
        {
            isGrappling = true;
            grappleTimer = grappleDuration;
            rb.gravityScale = 0f;
            currentGripPole.ExecuteLaunch(rb);
            //animator.SetTrigger("Grapple");
        }
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDist, groundLayer)
                      || Physics2D.Raycast(transform.position, Vector2.right, wallCheckDist, groundLayer);

        if (isGrappling)
        {
            grappleTimer -= Time.deltaTime;
            if (grappleTimer <= 0f)
            {
                isGrappling = false;
                rb.gravityScale = 1f; // Return gravity to normal
            }
            return; // SKIP the rest of the movement/gravity logic while grappling
        }
        
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.gravityScale = 1f;
            }
            return;
        }

        if (wallJumpCounter > 0)
        {
            wallJumpCounter -= Time.deltaTime;
        }
        else if (!isDashing && !isGrappling) // Only move if not wall jumping, dashing, or grappling
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }

        // Jump hold
        if (isJumpHeld && jumpAction.IsPressed() && rb.linearVelocity.y > 0)
        {
            if (jumpHoldTimer < maxJumpHoldTime)
            {
                rb.AddForce(Vector2.up * jumpHoldForce);
                jumpHoldTimer += Time.deltaTime;
            }
            else
            {
                isJumpHeld = false;
            }
        }

        handleWallSliding();
        flipSprite();

        // Gravity scaling
        bool atApex = Mathf.Abs(rb.linearVelocity.y) < apexThreshold && !isGrounded;
        if (isWallSliding)
        {
            rb.gravityScale = defaultGravity;
        }
        else if (atApex)
        {
            rb.gravityScale = apexGravityScale;
        }
        else if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = 4 * defaultGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        dashCooldownTimer -= Time.deltaTime;

        if (transform.position.y < -20f)
        {
            handleDeath();
            
        }
    }
    private void handleWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y <= 0)
        {
            if (!isWallSliding)
            {
                Debug.Log("Wall slide start");
                extraJumps = extraJumpsVal;
            }
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
        else
        {
            if (isWallSliding) Debug.Log("Wall slide end");
            isWallSliding = false;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            Debug.Log("Collide");
            handleDeath();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Damage")
        {
            handleDeath();
        }

        if (other.gameObject.tag == "GripPole")
        {
            currentGripPole = other.GetComponent<GripPole>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "GripPole")
        {
            currentGripPole = null;
        }
    }
    private void flipSprite()
    {
        if (moveInput.x < 0)
            spriteRenderer.flipX = false;
        else if (moveInput.x > 0)
            spriteRenderer.flipX = true;
    }
    private bool isDead = false;

    public void handleDeath()
    {
        if (isDead) return;
        isDead = true;
        DeathCounter.instance.AddDeath();
        SoundManager.instance.PlayDeath();
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        blackScreen.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);
        isDead = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}