using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jump (Celeste-like)")]
    public float jumpForce = 12f;           // initial jump velocity
    public float jumpHoldTime = 0.18f;      // how long holding jump will keep applying upward force
    public float coyoteTime = 0.12f;        // allow jump shortly after leaving ground
    public float jumpBufferTime = 0.12f;    // buffer jump input before landing
    public float fallMultiplier = 2.5f;     // gravity multiplier when falling
    public float lowJumpMultiplier = 2f;    // gravity multiplier for short hop

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    [Header("Climb")]
    public float climbSpeed = 4f;

    [Header("Respawn")]
    public Vector2 respawnPoint;

    // internals
    Rigidbody2D rb;
    float horizontal;
    bool isOnLadder = false;
    float defaultGravity;
    bool jumpInputHeld = false;

    // jump helpers
    float lastGroundedTime = -1f;     // last time we were on ground (for coyote)
    float lastJumpPressedTime = -1f;  // last time player pressed jump (for buffer)
    bool isJumping = false;
    float jumpTimeCounter = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        // read inputs
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpInputHeld = Input.GetButton("Jump");

        if (Input.GetButtonDown("Jump"))
        {
            lastJumpPressedTime = Time.time;
        }

        // ground detection (do in Update to update timers accurately)
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (grounded)
        {
            lastGroundedTime = Time.time;
        }

        // Attempt to start jump if buffered or within coyote time
        if ((Time.time - lastJumpPressedTime) <= jumpBufferTime)
        {
            if ((Time.time - lastGroundedTime) <= coyoteTime && !isOnLadder)
            {
                // start jump
                StartJump();
                lastJumpPressedTime = -999f;
            }
        }

        // climbing: handle input only in Update (physics applied in FixedUpdate)
        if (isOnLadder)
        {
            // while on ladder, ignore gravity
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        // If player releases jump early, stop variable jump hold
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimeCounter = 0f;
        }
    }

    void FixedUpdate()
    {
        // horizontal movement (applied in physics)
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        // ladder movement
        if (isOnLadder)
        {
            float v = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, v * climbSpeed);
            // keep gravity zero while climbing
            rb.gravityScale = 0f;
            return; // skip normal gravity modifications while on ladder
        }

        // variable jump height: if holding jump and still within jumpHoldTime, keep upward velocity
        if (isJumping)
        {
            if (jumpTimeCounter > 0f && jumpInputHeld)
            {
                // we already set initial velocity in StartJump(); optionally reinforce
                // small upward velocity maintain (optional, keep consistent)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimeCounter -= Time.fixedDeltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // Better falling physics (makes fall snappier like Celeste)
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !jumpInputHeld)
        {
            // short hop when player releases jump early
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    void StartJump()
    {
        // set vertical velocity directly (clean and responsive)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isJumping = true;
        jumpTimeCounter = jumpHoldTime;
        // reset coyote/buffer timers
        lastGroundedTime = -999f;
        lastJumpPressedTime = -999f;
    }

    // Ladder triggers
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            isOnLadder = true;
            // optional snap to ladder X position:
            // transform.position = new Vector3(col.transform.position.x, transform.position.y, transform.position.z);
        }

        // optional: checkpoint or other triggers could be here
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            isOnLadder = false;
            // restore gravity in next FixedUpdate
        }
    }

    // ground collision fallback (not used by main logic but kept for compatibility)
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            lastGroundedTime = Time.time;
        }
    }

    public void Die()
    {
        // simple respawn
        transform.position = respawnPoint;
        rb.linearVelocity = Vector2.zero;
    }

    // Debug: draw groundCheck in editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}



//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PlayerController : MonoBehaviour
//{

//    public float jumpForce = 12f;
//    public float jumpTime = 0.25f;
//    private float jumpTimeCounter;
//    private bool isJumping;

//    public Transform groundCheck;
//    public LayerMask groundLayer;

//    private Rigidbody2D rb;

//    public float moveSpeed = 5f;

//    private bool isGrounded = false;

//    public float climbSpeed = 5f;
//    private bool isOnLadder;
//    private float defaultGravity;

//    public Vector2 respawnPoint;
//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        defaultGravity = rb.gravityScale;

//    }

//    void Update()
//    {
//        // Horizontal movement
//        float h = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
//        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
//        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

//            if (isGrounded && Input.GetButtonDown("Jump"))
//    {
//        isJumping = true;
//        jumpTimeCounter = jumpTime;
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//    }

//    if (Input.GetButton("Jump") && isJumping)
//    {
//        if (jumpTimeCounter > 0)
//        {
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//            jumpTimeCounter -= Time.deltaTime;
//        }
//        else
//        {
//            isJumping = false;
//        }
//    }

//    if (Input.GetButtonUp("Jump"))
//    {
//        isJumping = false;
//    }


//        if (isOnLadder)
//        {
//            float vertical = Input.GetAxisRaw("Vertical");
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
//            rb.gravityScale = 0;
//        }
//        else
//        {
//            rb.gravityScale = defaultGravity;
//        }

//    }

//    private void OnTriggerEnter2D(Collider2D col)
//    {
//        if (col.CompareTag("Ladder"))
//            isOnLadder = true;
//    }

//    private void OnTriggerExit2D(Collider2D col)
//    {
//        if (col.CompareTag("Ladder"))
//            isOnLadder = false;
//    }


//    // Simple ground check using collisions
//    void OnCollisionEnter2D(Collision2D col)
//    {
//        if (col.gameObject.CompareTag("Ground"))
//            isGrounded = true;
//    }
//    void OnCollisionExit2D(Collision2D col)
//    {
//        if (col.gameObject.CompareTag("Ground"))
//            isGrounded = false;
//    }

//    public void Die()
//    {
//        transform.position = respawnPoint;
//    }
//}
