using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{ 
    public bool isMoving = true;
    public float movementSpeed = 1f;
    public float wallDistance=1f;
    public float cliffDistance=1f;
    public LayerMask detectionLayerMask;
    public bool alignToGround;
    public bool borderCheck;
    private bool flipped = false;
    private bool justEdged;
    private Vector2 lastNormal;
    private Rigidbody2D rb2d;
    public SpriteRenderer spriteRenderer;
    
    void Start()
    {
        rb2d=GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position , transform.up * -1, 1.5f, detectionLayerMask); 
        RaycastHit2D back = Physics2D.Raycast(transform.position - (flipped ? -transform.right : transform.right) * .1f, -transform.up, 1.5f, detectionLayerMask);

        if (hit)
        {
            if (borderCheck)
            {
                if (Physics2D.Raycast(transform.position, flipped ? Vector3.right * -1 : Vector3.right, wallDistance, detectionLayerMask))
                {
                    Switch();
                }

                if (!Physics2D.Raycast(transform.position + (flipped ? Vector3.right * -1 : Vector3.right), transform.up * -1, 1.5f, detectionLayerMask))
                {
                    Switch();
                }
            }

            if (alignToGround)
            {
                Vector2 normal;
                if (justEdged) {
                    if (Vector2.SignedAngle(hit.normal, lastNormal) == 0)
                    {
                        justEdged = false;
                        normal = hit.normal;
                    }
                    else { normal = lastNormal; }
                }
                else if (Vector2.SignedAngle(hit.normal, back.normal) != 0)
                {
                    normal = hit.normal;
                    lastNormal = normal;
                    justEdged = true;
                }
                else
                    normal = hit.normal;
                Debug.DrawRay(transform.position, normal,Color.green);
                Debug.DrawRay(transform.position, hit.normal,Color.yellow);
                Debug.DrawRay(transform.position - (flipped ? -transform.right : transform.right) * .1f, back.normal, Color.yellow);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Vector2.SignedAngle(Vector2.up,normal));
            }
            rb2d.velocity = transform.right * (flipped ? -movementSpeed : movementSpeed);
        }
    }

    private void Switch()
    {
        flipped = !flipped;
        spriteRenderer.flipX = flipped;
    }


}
