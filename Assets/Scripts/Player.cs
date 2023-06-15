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
    public float movementSpeed;
    public float jumpForce;
    

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private float movingInput;
    private bool isGrounded;


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

        if (playerUnlocked)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }


    }

    private void AnimatorControllers()
    {
        isRunning = rb.velocity.x != 0;
        playerAnim.SetFloat("xVelocity", rb.velocity.x);
        playerAnim.SetBool("isGrounded", isGrounded);
        playerAnim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        movingInput = Input.GetAxis("Horizontal");   

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }     
    }

  
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
