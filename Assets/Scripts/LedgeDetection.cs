using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Player player;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Enemy enemy;

    private bool canDetected;

    private BoxCollider2D boxCd => GetComponent<BoxCollider2D>();

    void Update()
    {
        if (player != null && canDetected)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
        }
        
        if (enemy != null && canDetected)
        {
            enemy.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
        }
    }

    //Los trigger permiten que la condicion sea falsa si detecta un muro desde arriba o desde los lados, ya que el box collider cubre la mitad superior del overlapcircle

    //Si el muro entra por debajo, la toca primero el overlapcircle que el trigger y eso resulta en true
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCd.bounds.center, boxCd.size, 0);

        foreach (var col in colliders)
        {
            if (col.gameObject.GetComponent<PlatformController>() != null)
            {
                return;
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
