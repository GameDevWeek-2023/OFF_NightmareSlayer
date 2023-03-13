using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Movement
    private Rigidbody2D rigidbody;
    private bool isGrounded = true;
    private float walkingVelocity;
    public float movementSpeed;
    public float jumpSpeed;
    //Input
    private PlayerInput playerInput;
    private bool canMove = true;
    //Attack
    private int attackDamage;
    //DreamShift
    private bool canDreamShift;
    private float dreamEssence;
    private float essenceCapacity;
    //Abilities
    private bool hasGlide;
    private bool hasDoubleJump;
    private bool usedDoubleJump;
    private bool hasGrappling;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        
        playerInput = new PlayerInput();

        playerInput.Movement.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Movement.Move.canceled += ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Movement.Jump.performed += ctx => Jump();
        playerInput.Movement.Jump.canceled += ctx => CancelJump();
        playerInput.Movement.Attack.performed += ctx => Attack();
        playerInput.Movement.Interact.performed += ctx => Interact();
        playerInput.Movement.DreamShift.performed += ctx => DreamShift();
        playerInput.Movement.Glide.performed += ctx => Glide(true);
        playerInput.Movement.Glide.canceled += ctx => Glide(false);
        playerInput.Movement.Grappling.performed += ctx => Grappling();

        playerInput.Movement.Enable();
    }

    private void Update()
    {
        //TODO prüfe ob grounded
        //TODO falls grounded, setze doublejump zurück
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(walkingVelocity * movementSpeed, rigidbody.velocity.y);
    }

    private void Move(Vector2 value)
    {
        if(!canMove) return;

        if (value.x < 0) //Walk left
        {
            walkingVelocity = -1f;
            if (transform.localScale.x > 0) transform.localScale = new Vector3(-1,1,1);
        }
        else if (value.x > 0) //Walk right
        {
            walkingVelocity = 1f;
            if (transform.localScale.x < 0) transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            walkingVelocity = 0f;
            if (value.y > 0) //Look Up
            {

            }
            else if (value.y < 0) //Look Down
            {

            }
            else //Idle
            {

            }
        }
    }

    private void Jump()
    {
        if(!canMove) return;

        if (isGrounded)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed);
        }
        else
        {
            if (!usedDoubleJump)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed);
                usedDoubleJump = true;
            }
        }
    }

    private void CancelJump()
    {
        if(!canMove) return;
        //if (isGrounded) return;

        float yVel = rigidbody.velocity.y;
        if (yVel > 0)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.4f*yVel);
    }

    private void Attack()
    {
        if(!canMove) return;
        //TODO lese Blickrichtung für Attackrichtung
    }

    private void Interact()
    {
        
    }

    private void DreamShift()
    {
        if(!canDreamShift) return;
        if(!canMove) return;
        
    }

    private void Glide(bool pressed)
    {
        if(!hasGlide) return;
        if(!canMove) return;
        
    }

    private void Grappling()
    {
        if(!hasGrappling) return;
        if(!canMove) return;
        
    }
}
