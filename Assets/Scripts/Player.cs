using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    private Animator playerAnim;

    [Header("Player Movement")]
    [SerializeField] private bool playerUnlocked;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;

    [Header("Slide Info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCooldown;
    private float slideCooldownCounter;
    private float slideTimerCounter;
    private bool isSliding;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isGrounded;
    private bool isWallDetected;


    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

    }

    void Update()
    {
        AnimatorControllers();
        CheckInput();
        CheckCollision();
        CheckForSlide();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if (playerUnlocked && !isWallDetected)
        {
            PlayerMovement();
        }

        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }

    private void CheckForSlide()
    {
        if (slideTimerCounter < 0)
        {
            isSliding = false;
        }
    }

    private void PlayerMovement()
    {
        if (isSliding)
        {
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
    }

    private void AnimatorControllers()
    {
        playerAnim.SetFloat("xVelocity", rb.velocity.x);
        playerAnim.SetFloat("yVelocity", rb.velocity.y);

        playerAnim.SetBool("isGrounded", isGrounded);
        playerAnim.SetBool("canDoubleJump", canDoubleJump);
        playerAnim.SetBool("isSliding", isSliding);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);

        isWallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0f, Vector2.zero, 0f, whatIsGround);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpMechanic();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlideMechanic();
        }
    }

    private void SlideMechanic()
    {
        if (rb.velocity.x != 0  && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void JumpMechanic()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));

        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
