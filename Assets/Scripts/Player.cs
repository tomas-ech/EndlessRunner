//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    private Animator playerAnim;
    private SpriteRenderer playerSprite;
    private bool isDead;
    [HideInInspector] public bool playerUnlocked;
    [HideInInspector] public bool extraLife;
    
    [Header("Knockback Info")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Move Info")]
    [SerializeField] private float speedToSurvive = 18;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    private float defaultMilestoneIncreaser;
    private float  defaultSpeed;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    private float speedMilestone;

    [Header("Jump Info")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;

    [Header("Slide Info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCooldown;
    [HideInInspector] public float slideCooldownCounter;
    private float slideTimerCounter;
    private bool isSliding;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float ceilingCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isGrounded;
    private bool isWallDetected;
    private bool ceilingDetected; 
    [HideInInspector] public bool ledgeDetected;

    [Header("Ledge Info")]
    [SerializeField] private Vector2 offset1; //beforeClimb
    [SerializeField] private Vector2 offset2; //AfterClimb

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

        speedMilestone = milestoneIncreaser;
        defaultSpeed = movementSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    void Update()
    {
        if (!playerUnlocked) {return;}

        CheckCollision();
        AnimatorControllers();
        SpeedController();
        CheckForSlide();
        CheckForLedge();
        CheckInput();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        extraLife = movementSpeed >= speedToSurvive;

        if (isDead) {return;}

        if (isKnocked) {return;}

        if (playerUnlocked)
        {
            PlayerMovement();
        }

        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }

#region Damage and Die
    public void Damage()
    {
        if (extraLife)
        {
            SpeedReset();
            KnockbackMechanic();
        }
        else {StartCoroutine(DeadMechanic());}
    }
    private IEnumerator DeadMechanic()
    {
        AudioManager.instance.PlaySFX(3);
        isDead = true;
        canBeKnocked = false;
        rb.velocity = knockbackDir;
        playerAnim.SetBool("isDead", true);

        Time.timeScale = 0.6f;

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        GameManager.instance.GameEnded();
    }

    private IEnumerator Invincibility()
    {
        Color originalColor = playerSprite.color;
        Color gettingHitColor = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.4f);

        canBeKnocked = false;
        playerSprite.color = gettingHitColor;
        yield return new WaitForSeconds(0.1f);

        playerSprite.color = originalColor;
        yield return new WaitForSeconds(0.1f);

        playerSprite.color = gettingHitColor;
        yield return new WaitForSeconds(0.15f);

        playerSprite.color = originalColor;
        yield return new WaitForSeconds(0.15f);

        playerSprite.color = gettingHitColor;
        yield return new WaitForSeconds(0.2f);

        playerSprite.color = originalColor;
        yield return new WaitForSeconds(0.2f);

        playerSprite.color = gettingHitColor;
        yield return new WaitForSeconds(0.25f);

        playerSprite.color = originalColor;
        canBeKnocked = true;
    }
#endregion

#region Knockback Mechanic
    private void KnockbackMechanic()
    {
        if (!canBeKnocked) {return;}

        StartCoroutine(Invincibility());
        isKnocked = true;
        rb.velocity = knockbackDir;
    }

    private void CancelKnockback() => isKnocked = false;
#endregion

#region SpeedControll
    private void SpeedReset()
    {
        if (isSliding) {return;}

        movementSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
    }
    private void SpeedController()
    {
        if (movementSpeed == maxSpeed)
        {
            return;
        }

        if (transform.position.x > speedMilestone)
        {
            speedMilestone += milestoneIncreaser;

            movementSpeed *= speedMultiplier;
            milestoneIncreaser *= speedMultiplier;

            if (movementSpeed > maxSpeed)
            {
                movementSpeed = maxSpeed;
            }
        }
    }
#endregion

#region Slide Mechanic
    private void CheckForSlide()
    {
        if (slideTimerCounter < 0 && !ceilingDetected)
        {
            isSliding = false;
        }
    }
    public void SlideMechanic()
    {
        if (rb.velocity.x != 0  && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCooldownCounter = slideCooldown;
        }
    }
#endregion

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

#region Player Movement
    private void PlayerMovement()
    {

        if (isWallDetected)
        {
            SpeedReset();
            return;
        }

        if (isSliding)
        {
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
    }
    public void JumpMechanic()
    {

        if (isSliding)
        {
            return; //Al poner return en este condicional, los demas condicionales
            //en esta misma funcion no se van a activar
        }

        RollAnimationFinished();

        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.instance.PlaySFX(Random.Range(1,2));
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            AudioManager.instance.PlaySFX(Random.Range(1,2));
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
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
#endregion

#region Animations
    private void AnimatorControllers()
    {
        playerAnim.SetFloat("xVelocity", rb.velocity.x);
        playerAnim.SetFloat("yVelocity", rb.velocity.y);

        playerAnim.SetBool("isGrounded", isGrounded);
        playerAnim.SetBool("canDoubleJump", canDoubleJump);
        playerAnim.SetBool("isSliding", isSliding);
        playerAnim.SetBool("canClimb", canClimb);
        playerAnim.SetBool("isKnocked", isKnocked);
        
        if (rb.velocity.y < -25)
        {
            playerAnim.SetBool("canRoll", true);
        }
    }

    private void RollAnimationFinished() => playerAnim.SetBool("canRoll", false);
#endregion

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);

        isWallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0f, Vector2.zero, 0f, whatIsGround);

        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));

        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
