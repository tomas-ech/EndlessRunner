using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator enemyAnim;
    public bool canMove; 
    private Player player;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float distanceToRun;
    private float maxDistance;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float ceilingCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundForwardCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isGrounded;
    [SerializeField] private bool groundForward;
    private bool isWallDetected;
    private bool ceilingDetected; 
    public bool ledgeDetected = false;

    [Header("Ledge Info")]
    [SerializeField] private Vector2 offset1; //beforeClimb
    [SerializeField] private Vector2 offset2; //AfterClimb

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb = false; 



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        player = GameManager.instance.playerScript;

        maxDistance = transform.position.x + distanceToRun;
    }

    void Update()
    {

        canMove = GameManager.instance.playerScript.playerUnlocked;

        CheckCollision();
        AnimatorController();
        Movement();
        CheckForLedge();
        SpeedController();

        if (transform.position.x > maxDistance)
        {
            canMove = false;
        }


        if (isGrounded && !groundForward || isWallDetected)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    private void SpeedController()
    {
        bool playerAhead = player.transform.position.x > transform.position.x;
        bool playerFarAway = Vector2.Distance(player.transform.position, transform.position) > 6f;

        if (playerAhead)
        {
            if (playerFarAway)
            {
                moveSpeed = 22;
            }
            else
            {
                moveSpeed = 16;
            }
        }
        else
        {
            if (playerFarAway)
            {
                moveSpeed = 11;
            }
            else
            {
                moveSpeed = 14;
            }
        }
    }

    private void Movement()
    {
        if (canMove) {rb.velocity = new Vector2(moveSpeed, rb.velocity.y);}
        else {rb.velocity = new Vector2(0, rb.velocity.y);}
    }

    #region Climb Mechanic
    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            rb.gravityScale = 0;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }

        if (canClimb)
        {
            transform.position = climbBegunPosition;
        }
    }
    private void LedgeClimbMechanic()
    {
        canClimb = false;
        rb.gravityScale = 5;
        transform.position = climbOverPosition;
        //Invoke nos permite usar una funcion con cierto delay para empezar
        Invoke("AllowLedgeGrab", 0.2f); 
    }

    //Esta funcion por ser muy corta se puede usar => indicando que va a retornar un solo valor, en este caso un booleano
    private void AllowLedgeGrab() => canGrabLedge = true;

#endregion

    private void AnimatorController()
    {
        enemyAnim.SetFloat("xVelocity", rb.velocity.x);
        enemyAnim.SetFloat("yVelocity", rb.velocity.y);

        enemyAnim.SetBool("isGrounded", isGrounded);
        enemyAnim.SetBool("canClimb", canClimb);

    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        groundForward = Physics2D.Raycast(groundForwardCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        isWallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0f, Vector2.zero, 0f, whatIsGround);

        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(groundForwardCheck.position, new Vector2(groundForwardCheck.position.x, groundForwardCheck.position.y - groundCheckDistance));

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));

        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
