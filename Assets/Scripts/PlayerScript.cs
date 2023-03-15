using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;
    
    //DEBUG
    public bool godMode = false;
    
    //UI
    public GameObject pausePanel;
    public GameObject playerStats;
    public GameObject dialogueObject;
    public TextMeshProUGUI dialogueText;
    private float dialogueSpeed;
    private Coroutine dialogueCoroutine;
    private int dialogueState;
    public HeartManager heartManager;
    public EssenzManager essenzManager;

    //Movement
    private new Rigidbody2D rigidbody;
    private new CapsuleCollider2D collider;
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
    private Interactable currentInteractable;
    
    //Attack
    private int attackDamage = 3;
    private float attackRange = .7f;
    private bool canGetDamage = true;
    public LayerMask hittableLayers;
    private float attackHitKnockback = 10f;
    private bool movedAfterHit = true;

    //Hitpoints
    private int lifes = 4;
    private int maxLifes = 6;

    private SpriteRenderer spriteRenderer;
    
    //DreamShift
    private bool canDreamShift = true;
    private float dreamEssence = 2.4f;
    private float essenceCapacity = 3;
    private bool cancelShift;
    
    //Abilities
    private bool hasGlide = true;
    private bool hasDoubleJump = true;
    private bool usedDoubleJump;
    private bool hasGrappling = true;
    private bool usingGrappling;
    private List<Grappable> grappableTargets;
    private Grappable currentTarget;
    public LayerMask obstaclesTowardsTarget;
    private bool movedAfterGrappling = true;
    private Animator animator;

    private void Awake()
    {
        instance = this;
        
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        dialogueText.text = "";
        dialogueObject.SetActive(false);
        
        SetUILives();
        SetUIEssenzBar();
        playerStats.SetActive(true);

        grappableTargets = new List<Grappable>();
        
        playerInput = new PlayerInput();

        playerInput.Movement.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Movement.Move.canceled += ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Movement.Jump.performed += ctx => Jump();
        playerInput.Movement.Jump.canceled += ctx => CancelJump();
        playerInput.Movement.Attack.performed += ctx => Attack();
        playerInput.Movement.Interact.performed += ctx => Interact();
        playerInput.Movement.DreamShift.performed += ctx => DreamShift();
        playerInput.Movement.DreamShift.canceled += ctx => CancelDreamShift();
        playerInput.Movement.Glide.performed += ctx => Glide(true);
        playerInput.Movement.Glide.canceled += ctx => Glide(false);
        playerInput.Movement.Grappling.performed += ctx => Grappling();
        playerInput.Movement.Pause.performed += ctx => Pause();

        playerInput.Movement.Enable();

        lifes = maxLifes;
    }

    private void Update()
    {
        RaycastHit2D leftSideRay = Physics2D.Raycast(transform.position + Vector3.left * .15f, Vector2.down, .85f, groundLayerMask);
        RaycastHit2D rightSideRay = Physics2D.Raycast(transform.position + Vector3.right * .15f, Vector2.down, .85f, groundLayerMask);
        
        if (leftSideRay || rightSideRay)
        {
            isGrounded = true;
            if(!usingGrappling) movedAfterGrappling = true;
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
        
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position + Vector3.down * .3f,.7f);
        
        Gizmos.color = Color.magenta;
        //Gizmos.DrawLine(transform.position + Vector3.up * .42f + Vector3.left * .34f, transform.position + Vector3.up * .42f + Vector3.right * .34f);
        //Gizmos.DrawLine(transform.position + Vector3.down * .78f + Vector3.left * .34f, transform.position + Vector3.down * .78f + Vector3.right * .34f);
        //Gizmos.DrawSphere(transform.position + new Vector3(collider.offset.x,collider.offset.y),.5f);

        if (grappableTargets == null) return;
        foreach (var target in grappableTargets)
        {
            Gizmos.DrawLine(transform.position,target.transform.position);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D leftSideRay = Physics2D.Raycast(transform.position + Vector3.left * .2f, Vector2.down, .85f, nonGroundLayerMask);
        RaycastHit2D rightSideRay = Physics2D.Raycast(transform.position + Vector3.right * .2f, Vector2.down, .85f, nonGroundLayerMask);

        if (usingGrappling) return;
        if (isGliding && !isGrounded)
        {
            if (walkingVelocity == 0)
            {
                if(!(transform.localScale.x > 0 ? rightSideRay : leftSideRay))
                    rigidbody.velocity = new Vector2(transform.localScale.x * glidingSpeed, -glideFallSpeed);
                else
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, -glideFallSpeed);
                }
            }
            else
            {
                if(!(walkingVelocity > 0 ? rightSideRay : leftSideRay))
                    rigidbody.velocity = new Vector2(walkingVelocity * glidingSpeed, -glideFallSpeed);
                else
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, -glideFallSpeed);
                }
            }
        }
        else
        {
            if (!movedAfterGrappling || !movedAfterHit) return;
            if (walkingVelocity == 0 && !leftSideRay && !rightSideRay)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
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

        
        //Target finden für Grappling
        if(!hasGrappling) return;
        
        Vector2 ownPos = transform.position;
        float distanceToTarget = float.MaxValue;
        if(currentTarget != null) currentTarget.Untarget();
        currentTarget = null;
        if (usingGrappling) return;
        foreach (var target in grappableTargets)
        {
            Vector2 targetPos = target.transform.position;
            if(targetPos.y < ownPos.y) continue;
            if (transform.localScale.x > 0 ? (targetPos.x < ownPos.x) : (targetPos.x > ownPos.x)) continue;

            float currentDistance = Vector2.Distance(ownPos,targetPos);
            RaycastHit2D hit = Physics2D.Raycast(ownPos, targetPos-ownPos, currentDistance, obstaclesTowardsTarget);
            if(hit)
            {
                continue;
            }
                
            if (currentDistance < distanceToTarget)
            {
                distanceToTarget = currentDistance;
                currentTarget = target;
            }
        }
        if(currentTarget != null) currentTarget.Target();
    }

    private void Move(Vector2 value)
    {
        if(!canMove) return;

        movedAfterGrappling = true;
        movedAfterHit = true;

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

        movedAfterGrappling = true;
        movedAfterHit = true;

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

    public void GetDamage()
    {
        if (!canGetDamage) return;
        
        if(!godMode) lifes--;
        SetUILives();
        
        //TODO hit effects

        rigidbody.velocity = Vector2.zero;
        StartCoroutine(Invinsibility(1.5f));
    }

    private IEnumerator Invinsibility(float time)
    {
        canGetDamage = false;
        float timeOver = 0f;

        while (timeOver < time)
        {
            timeOver += Time.deltaTime;

            spriteRenderer.enabled = Mathf.RoundToInt(timeOver * 4) % 2 == 0;
            
            yield return 0;
        }

        spriteRenderer.enabled = true;
        canGetDamage = true;
    }

    public void GetDamage(Vector2 knockback)
    {
        if (!canGetDamage) return;
        GetDamage();
        
        movedAfterHit = false;
        rigidbody.velocity =  knockback * attackHitKnockback;
    }

    private void Attack()
    {
        if(!canMove) return;

        Vector2 look = playerInput.Movement.Move.ReadValue<Vector2>();
        if (look.x < 0)
        {
            //Debug.Log("Attacke Links");
            DoAttack(0,attackRange);
            animator.SetTrigger("hit");
        }
        else if (look.x > 0)
        {
            //Debug.Log("Attacke Rechts");
            DoAttack(1,attackRange);
            animator.SetTrigger("hit");
        }
        else if (look.y > 0)
        {
            //Debug.Log("Attacke Oben");
            DoAttack(2,attackRange);
        }
        else if (look.y < 0)
        {
            //Debug.Log("Attacke Unten");
            DoAttack(3,attackRange);
        }
        else
        {
            if (transform.localScale.x < 0)
            {
                //Debug.Log("Attacke Links");
                DoAttack(0,attackRange);
                animator.SetTrigger("hit");
            }
            else
            {
                //Debug.Log("Attacke Rechts");
                DoAttack(1,attackRange);
                animator.SetTrigger("hit");
            }
        }
        if(isGliding) CancelGliding();
    }

    private void DoAttack(int direction, float radius)
    {
        movedAfterHit = false;
        
        //links rechts oben unten
        //0     1      2    3
        Vector3[] attackPoints = {
            transform.position + Vector3.left * .4f + Vector3.down * .25f,
            transform.position + Vector3.right * .4f + Vector3.down * .25f,
            transform.position + Vector3.up * .2f,
            transform.position + Vector3.down * .4f};
        Vector3 attackPoint = attackPoints[direction];
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint, radius, hittableLayers);
        
        if (enemiesHit.Length == 0) return; //No Hit
        foreach (var enemy in enemiesHit)
        {
            HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(attackDamage);
            }
        }
        
        //TODO do sfx stuff to indicate hit

        Vector2[] knockbackVectors = { Vector2.right, Vector2.left, Vector2.down, Vector2.up };
        rigidbody.velocity =  knockbackVectors[direction] * attackHitKnockback * (direction < 2 ? .7f:1f);
    }

    private void Interact()
    {
        if (dialogueCoroutine != null)
        {
            if(dialogueState == 1)
            {
                dialogueState = 2;
                return;
            }
            if(dialogueState == 2)
            {
                canMove = true;
                dialogueObject.SetActive(false);
                dialogueText.text = "";
                dialogueCoroutine = null;
                playerStats.SetActive(true);
                return;
            }
        }
        
        if(!canMove) return;
        
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
            walkingVelocity = 0;
        }
    }

    private void DreamShift()
    {
        if(!canDreamShift) return;
        if(!canMove) return;
        //if(!isGrounded) return;

        if (godMode)
        {
            GameManager.instance.SwitchNightmare();
        }
        else if (dreamEssence >= 1f)
        {
            dreamEssence -= 1f;
            SetUIEssenzBar();
            GameManager.instance.SwitchNightmare();
        }

        //StartCoroutine(DreamShifting());
    }

    /*private IEnumerator DreamShifting()
    {
        canMove = false;
        walkingVelocity = 0f;
        cancelShift = false;

        const float consumingRate = 0.5f;
        float consumed = 0f;
        
        while (!cancelShift)
        {
            float amt = Time.deltaTime * consumingRate;
            
            dreamEssence -= amt;
            consumed += amt;

            if (dreamEssence <= 0f)
            {
                dreamEssence = 0f;
                SetUIEssenzBar();
                break;
            }
            if (consumed >= 1f)
            {
                dreamEssence += consumed - 1f;
                SetUIEssenzBar();
                GameManager.instance.SwitchNightmare();

                canMove = true;
                Move(playerInput.Movement.Move.ReadValue<Vector2>());
                yield break;
            }
            SetUIEssenzBar();

            yield return new WaitForEndOfFrame();
        }
        //TODO Camera zurücksetzen
        //TODO Particle stoppen

        canMove = true;
        Move(playerInput.Movement.Move.ReadValue<Vector2>());
    }*/

    private void CancelDreamShift()
    {
        cancelShift = true;
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
        if (usingGrappling) return;
        if (currentTarget == null) return;

        StartCoroutine(UseGrappling());
    }

    private IEnumerator UseGrappling()
    {
        Grappable target = currentTarget;
        
        usingGrappling = true;
        canMove = false;
        movedAfterGrappling = false;

        rigidbody.gravityScale = 0;
        rigidbody.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(.2f);
        Vector2 direction = target.transform.position - transform.position;
        Vector2 newVelocity = direction.normalized * 16;
        rigidbody.velocity = newVelocity;
        yield return new WaitForSeconds(direction.magnitude/newVelocity.magnitude);
        
        rigidbody.gravityScale = 1;
        usingGrappling = false;
        canMove = true;
        
        if (playerInput.Movement.Move.IsPressed())
        {
            Move(playerInput.Movement.Move.ReadValue<Vector2>());
        }
        else
        {
            walkingVelocity = 0;
        }
        
    }

    private void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grappable"))
        {
            Grappable target = other.GetComponent<Grappable>();
            if(!grappableTargets.Contains(target)) grappableTargets.Add(target);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            currentInteractable = other.GetComponent<Interactable>();
            if(isGrounded)
            {
                currentInteractable.ShowText();
            }
            else
            {
                currentInteractable.HideText();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && other.GetComponent<Interactable>() == currentInteractable)
        {
            currentInteractable.HideText();
            currentInteractable = null;
        }
        
        if (other.CompareTag("Grappable"))
        {
            Grappable target = other.GetComponent<Grappable>();
            grappableTargets.Remove(target);
        }
    }

    public void SetDialogueSpeed(float speed)
    {
        dialogueSpeed = speed;
    }

    public void StartDialogue(string sentence)
    {
        dialogueCoroutine = StartCoroutine(Writer(sentence));
    }
    
    private IEnumerator Writer(string sentence)
    {
        dialogueText.text = "";
        dialogueObject.SetActive(true);
        canMove = false;
        dialogueState = 1;
        playerStats.SetActive(false);

        foreach (char c in sentence) 
        {
            if (dialogueState == 2)
            {
                dialogueText.text = sentence;
                break;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        dialogueState = 2;
    }
    
    //UI-Methods

    private void SetUILives()
    {
        heartManager.SetHearts(lifes,maxLifes);
    }

    private void SetUIEssenzBar()
    {
        if (dreamEssence > essenceCapacity) dreamEssence = essenceCapacity;
        float zs = dreamEssence / essenceCapacity;
        essenzManager.SetEssenz(zs);
    }
}
