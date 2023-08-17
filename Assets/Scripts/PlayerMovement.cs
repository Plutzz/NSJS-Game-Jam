using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 20f;
    public float deceleration = 20f;
    public float jumpForce = 10f;
    public float variableJumpMultiplier = 0.5f;
    public float maxJumpDuration = 0.2f;
    public float coyoteTimeDuration = 0.1f;
    public float maxFallSpeed = -10f;
    public float jumpApexDuration = 0.2f;
    public float jumpApexHangTime = 0.1f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpStartTime;
    private bool coyoteTimeActive;
    private float coyoteTimeStart;
    private float moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Input handling
        moveInput = Input.GetAxis("Horizontal");

        // Coyote time logic
        if (isGrounded)
        {
            coyoteTimeActive = true;
            coyoteTimeStart = Time.time;
        }
        else if (Time.time - coyoteTimeStart > coyoteTimeDuration)
        {
            coyoteTimeActive = false;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            EndJump();
        }
    }

    private void FixedUpdate()
    {
        // Apply acceleration and deceleration
        float targetVelocity = moveInput * moveSpeed;

        if (isGrounded)
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, targetVelocity, acceleration * Time.fixedDeltaTime), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, targetVelocity, deceleration * Time.fixedDeltaTime), rb.velocity.y);
        }

        // Apply max fall speed
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }

    private void Jump()
    {
        if (coyoteTimeActive)
        {
            isJumping = true;
            jumpStartTime = Time.time;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void EndJump()
    {
        if (isJumping)
        {
            isJumping = false;

            float timeSinceJumpStart = Time.time - jumpStartTime;

            if (rb.velocity.y > 0 && timeSinceJumpStart < maxJumpDuration)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpMultiplier);
            }
        }
    }
}
