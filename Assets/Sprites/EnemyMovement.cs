using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour
{
    public bool isMoving = true;
    public float movementSpeed = 1f;
    public float wallDistance=1f;
    public float cliffDistance=1f;
    public LayerMask detectionLayerMask;
    private bool flipped = false;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb2d=GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (Physics2D.Raycast(transform.position, transform.up * -1, 1.5f, detectionLayerMask))
        {
            rb2d.velocity = new Vector3(flipped ? -movementSpeed : movementSpeed, rb2d.velocity.y);

            if (Physics2D.Raycast(transform.position, flipped ? transform.right * -1 : transform.right, wallDistance, detectionLayerMask))
            {
                Switch();
            }

            if (!Physics2D.Raycast(transform.position + (flipped ? transform.right * -1 : transform.right), transform.up * -1, 1.5f, detectionLayerMask))
            {
                Switch();
            }
        }
    }

    private void Switch()
    {
        flipped = !flipped;
        spriteRenderer.flipY = flipped;
    }


}
