using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBoss : Boss
{
    public LayerMask ground;
    public LayerMask player;
    public float jumpStrength = 10f;
    public float jumpStrengthSide = 10f;
    public float jumpDelta=3f;
    private float jumpCoolDown = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {

            rb2d.velocity = (PlayerScript.instance.transform.position - transform.position).normalized * 10;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
        /*if (jumpCoolDown == -1)
        {
            jumpCoolDown = jumpDelta;
        }
        else if (jumpCoolDown < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector3.down, 2.5f, ground))
            {
                Jump();
                jumpCoolDown = -1;
            }
        }
        else
        {
            jumpCoolDown -= Time.deltaTime;
        }*/

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

        rb2d.velocity = new Vector2(sideVelocity * Random.Range(.5f, 1.5f),jumpStrength * Random.Range(.5f, 1.5f));
    }
}
