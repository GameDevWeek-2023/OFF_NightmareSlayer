using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //UI
    public GameObject pausePanel;
    
    //Movement
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private bool isGrounded;
    private bool isGliding;
    private float walkingVelocity;
    public float movementSpeed;
    public float jumpSpeed;
    public float glidingSpeed;
    public float glideFallSpeed;
    public LayerMask groundLayerMask;
    public LayerMask nonGroundLayerMask;
    public PhysicsMaterial2D physicsMaterialAir;
    public PhysicsMaterial2D physicsMaterialGround;
    public PhysicsMaterial2D physicsMaterialWalk;
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
    private bool hasGlide = true;
    private bool hasDoubleJump = true;
    private bool usedDoubleJump;
    private bool hasGrappling = true;
    private Animator animator;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
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
        playerInput.Movement.Pause.performed += ctx => Pause();

        playerInput.Movement.Enable();
    }

    private void Update()
    {
        RaycastHit2D leftSideRay = Physics2D.Raycast(transform.position + Vector3.left * .15f, Vector2.down, .85f, groundLayerMask);
        RaycastHit2D rightSideRay = Physics2D.Raycast(transform.position + Vector3.right * .15f, Vector2.down, .85f, groundLayerMask);
        
        if (leftSideRay || rightSideRay)
        {
            isGrounded = true;
            usedDoubleJump = false;
            if(isGliding) CancelGliding();
        }
        else
        {
            isGrounded = false;
        }


        if (!isGrounded)
        {
            collider.sharedMaterial = physicsMaterialAir;
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            if (walkingVelocity != 0)
            {
                collider.sharedMaterial = physicsMaterialWalk;
                animator.SetBool("isRunning", true);
            }
            else
            {
                collider.sharedMaterial = physicsMaterialGround;
                animator.SetBool("isRunning", false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * .15f, transform.position + Vector3.left * .15f + Vector3.down * .85f);
        Gizmos.DrawLine(transform.position + Vector3.right * .15f, transform.position + Vector3.right * .15f + Vector3.down * .85f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.left * .2f, transform.position + Vector3.left * .2f + Vector3.down * .85f);
        Gizmos.DrawLine(transform.position + Vector3.right * .2f, transform.position + Vector3.right * .2f + Vector3.down * .85f);
    }

    private void FixedUpdate()
    {
        RaycastHit2D leftSideRay = Physics2D.Raycast(transform.position + Vector3.left * .2f, Vector2.down, .85f, nonGroundLayerMask);
        RaycastHit2D rightSideRay = Physics2D.Raycast(transform.position + Vector3.right * .2f, Vector2.down, .85f, nonGroundLayerMask);

        if (isGliding && !isGrounded)
        {
            if (walkingVelocity != 0)
            {
                rigidbody.velocity = new Vector2(walkingVelocity * glidingSpeed, -glideFallSpeed);
            }
            else
            {
                rigidbody.velocity = new Vector2(transform.localScale.x * glidingSpeed, -glideFallSpeed);
            }
        }
        else
        {
            if (walkingVelocity == 0 && !leftSideRay && !rightSideRay)
            {
                rigidbody.velocity = new Vector2(walkingVelocity * movementSpeed, rigidbody.velocity.y);
            }
            else
            {
                if (!(walkingVelocity > 0 ? rightSideRay : leftSideRay))
                {
                    rigidbody.velocity = new Vector2(walkingVelocity * movementSpeed, rigidbody.velocity.y);
                }
            }
        }
    }

    private void LateUpdate()
    {
        //soll transformation von horizontaler in vertikaler velocity an Schrägen verhindern
        if (isGrounded && playerInput.Movement.Move.WasReleasedThisFrame() && !playerInput.Movement.Jump.WasPerformedThisFrame())
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        }
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
            if (hasDoubleJump && !usedDoubleJump)
            {
                if(isGliding) CancelGliding();
                rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed);
                usedDoubleJump = true;
            }
        }
    }

    private void CancelJump()
    {
        if(!canMove) return;

        float yVel = rigidbody.velocity.y;
        if (yVel > 0)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.4f*yVel);
    }

    private void Attack()
    {
        if(!canMove) return;

        Vector2 look = playerInput.Movement.Move.ReadValue<Vector2>();
        if (look.x < 0)
        {
            Debug.Log("Attacke Links");
        }
        else if (look.x > 0)
        {
            Debug.Log("Attacke Rechts");
        }
        else if (look.y > 0)
        {
            Debug.Log("Attacke Oben");
        }
        else if (look.y < 0)
        {
            Debug.Log("Attacke Unten");
        }
        else
        {
            if (transform.localScale.x < 0)
            {
                Debug.Log("Attacke Links");
            }
            else
            {
                Debug.Log("Attacke Rechts");
            }
        }
        if(isGliding) CancelGliding();
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

        if (isGliding && !pressed)
        {
            CancelGliding();
        }
        else if (!isGliding && pressed && !isGrounded)
        {
            StartGliding();
        }
    }

    private void StartGliding()
    {
        isGliding = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0f;
        //TODO Gliding Animation
    }

    private void CancelGliding()
    {
        isGliding = false;
        rigidbody.gravityScale = 1f;
        //TODO Stop Gliding Animation
    }

    private void Grappling()
    {
        if(!hasGrappling) return;
        if(!canMove) return;
        
    }

    private void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
}
