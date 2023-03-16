using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAround : BossAttack
{
    public LayerMask ground;
    public LayerMask player;
    public float jumpStrength = 10f;
    public float jumpStrengthSide = 10f;
    public float jumpDelta = 3f;
    private float jumpCoolDown = -1;
    private Rigidbody2D rb2d;
    private bool lastFrameGrounded = false;
    void Awake()
    {
        base.Awake();
        rb2d=GetComponent<Rigidbody2D>();
    }
    public override void AttackUpdate()
    {
        
        if (jumpCoolDown == -1)
        {
            //if (!lastFrameGrounded)
            //    boss.bossroomManager.CameraShake(.5f);
            jumpCoolDown = jumpDelta;
        }
        else if (jumpCoolDown < 0)
        {
            if (Random.Range(0, 3) == 0)
            {
                Finished();
            }
            else if (Physics2D.Raycast(transform.position, Vector3.down, 4f, ground))
            {
                Jump();
                
                lastFrameGrounded = true;

                jumpCoolDown = -1;
            }
            else lastFrameGrounded=false;
        }
        else
        {
            jumpCoolDown -= Time.deltaTime;
        }

    }

    private void Jump()
    {

        float sideVelocity = 0;
        if (transform.localPosition.x < 0)
        {
            sideVelocity = jumpStrengthSide;

        }
        else
        {
            sideVelocity = -jumpStrengthSide;

        }

        rb2d.velocity = new Vector2(sideVelocity * Random.Range(.5f, 1.5f), jumpStrength * Random.Range(.5f, 1.5f));
    }
}
