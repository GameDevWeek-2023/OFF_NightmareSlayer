using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

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
    private bool abilityGranted;
    public AbilityScreen abilityScreen;
    public HeartManager heartManager;
    public EssenzManager essenzManager;
    public TextMeshProUGUI coinsText;
    public GameObject deathScreen;
    public GameObject afterDeathButton;

    //Movement
    private new Rigidbody2D rigidbody;
    private new CapsuleCollider2D collider;
    private bool isGrounded;
    private bool isGliding;
    private float walkingVelocity;
    public float movementSpeed;
    public float jumpSpeed;
    public float ledgeForgivenessTime;
    private bool canDash;
    public float dashTime;
    public float dashSpeed;
    public float dashCooldown;
    public float glidingSpeed;
    public float glideFallSpeed;
    public LayerMask groundLayerMask;
    public LayerMask nonGroundLayerMask;
    public PhysicsMaterial2D physicsMaterialAir;
    public PhysicsMaterial2D physicsMaterialGround;
    public PhysicsMaterial2D physicsMaterialWalk;
    private Coroutine jumpDelay;
    private Coroutine recallCoroutine;
    private float recallTime = 2f;
    
    //Input
    private PlayerInput playerInput;
    private bool canMove = true;
    private Interactable currentInteractable;
    
    //Attack
    private int attackDamage = 3;
    private float attackRange = .8f;
    private bool canGetDamage = true;
    private bool canAttack = true;
    private float attackCooldown = .16f;
    public LayerMask hittableLayers;
    private float attackHitKnockback = 10f;
    private float attackHitEnemyKnockback = 5f;
    private bool movedAfterHit = true;

    //Hitpoints
    private int lifes;
    private int maxLifes = 4;

    private SpriteRenderer spriteRenderer;
    
    //DreamShift
    private bool canDreamShift = true;
    private float dreamEssence;
    private float essenceCapacity = 3;
    private bool cancelShift;
    
    //Abilities
    private int hasGlide = 0;
    private int hasDoubleJump = 0;
    private bool usedDoubleJump;
    private int hasGrappling = 0;
    private bool movementLocked;
    private List<Grappable> grappableTargets;
    private Grappable currentTarget;
    public LayerMask obstaclesTowardsTarget;
    private bool movedAfterGrappling = true;
    private Animator animator;
    
    //Coins
    private int coins;
    private float coinLossFactor = .5f;
    
    //Sounds
    public GameObject audioObject;
    public List<AudioClip> attackSounds;
    public AudioClip jumpSound;

    //Special Effects
    public GameObject doubleJumpPS;
    public GameObject dashPSLeft;
    public GameObject dashPSRight;
    public GameObject dreamShiftPS;

    private void OnDestroy()
    {
        playerInput.Movement.Disable();
    }

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeStats();
        
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
        playerInput.Movement.Recall.performed += ctx => RecallStart();
        playerInput.Movement.Recall.canceled += ctx => CancelRecall();

        playerInput.Movement.Enable();
    }

    private void Start()
    {
        ToSpawnPoint();
    }

    private void InitializeStats()
    {
        deathScreen.SetActive(false);
        
        dashPSLeft.SetActive(false);
        dashPSRight.SetActive(false);

        dialogueText.text = "";
        dialogueObject.SetActive(false);
        
        abilityScreen.gameObject.SetActive(false);
        
        if(currentTarget != null)
        {
            currentTarget.Untarget();
            currentTarget = null;
        }
        
        grappableTargets = new List<Grappable>();

        lifes = maxLifes;
        dreamEssence = 1f;
        SetUILives();
        SetUIEssenzBar();
        SetUICoins();
        playerStats.SetActive(true);

        canDash = true;
        canMove = true;
        canAttack = true;
        canGetDamage = true;
    }

    private void Update()
    {
        RaycastHit2D leftSideRay = Physics2D.Raycast(transform.position + Vector3.left * .15f, Vector2.down, .85f, groundLayerMask);
        RaycastHit2D rightSideRay = Physics2D.Raycast(transform.position + Vector3.right * .15f, Vector2.down, .85f, groundLayerMask);
        
        if (leftSideRay || rightSideRay)
        {
            if(jumpDelay != null)
            {
                StopCoroutine(jumpDelay);
                jumpDelay = null;
            }
            isGrounded = true;
            if(!movementLocked) movedAfterGrappling = true;
            usedDoubleJump = false;
            if(isGliding) CancelGliding();
        }
        else
        {
            if (jumpDelay == null) jumpDelay = StartCoroutine(JumpDelay());
        }


        if (!isGrounded || movementLocked)
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

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(ledgeForgivenessTime);
        isGrounded = false;
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
        //Gizmos.DrawWireSphere(transform.position + Vector3.right * transform.localScale.x * .6f + Vector3.down * .25f,attackRange);
        
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

        if (movementLocked) return;
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
        if(!CanUseAbility(AbilityType.Grappling)) return;
        
        Vector2 ownPos = transform.position;
        float distanceToTarget = float.MaxValue;
        //if(currentTarget != null) currentTarget.Untarget();
        //currentTarget = null;

        Grappable newTarget = null;
        
        if (movementLocked) return;
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
                newTarget = target;
            }
        }

        if (currentTarget != null && newTarget != currentTarget)
        {
            currentTarget.Untarget();
        }

        if (newTarget != null && newTarget != currentTarget)
        {
            newTarget.Target();
        }

        currentTarget = newTarget;
    }

    private void Move(Vector2 value)
    {
        if(!canMove) return;

        movedAfterGrappling = true;
        movedAfterHit = true;

        if (value.x < 0) //Walk left
        {
            walkingVelocity = -1f;
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (value.x > 0) //Walk right
        {
            walkingVelocity = 1f;
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
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

    private void CheckForMove()
    {
        Move(playerInput.Movement.Move.ReadValue<Vector2>());
    }

    private void Jump()
    {
        if(!canMove) return;

        movedAfterGrappling = true;
        movedAfterHit = true;

        if (isGrounded) //Jump
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed);
            //PlayAudio(jumpSound);
        }
        else
        {
            if (CanUseAbility(AbilityType.DoubleJump) && !usedDoubleJump) //Double Jump
            {
                if(isGliding) CancelGliding();
                rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed);
                usedDoubleJump = true;
                Instantiate(doubleJumpPS,transform.position, Quaternion.Euler(0,0,-115));
                PlayAudio(jumpSound);
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

    public void Heal()
    {
        //TODO effect stuff when healing
        lifes++;
        if (lifes > maxLifes) lifes = maxLifes;
        SetUILives();
    }

    public void FullHeal()
    {
        lifes = maxLifes;
        SetUILives();
    }

    public void GetDamage()
    {
        if (!canGetDamage) return;
        
        CancelRecall();
        if(!godMode) lifes--;
        SetUILives();

        //TODO hit effects

        rigidbody.velocity = Vector2.zero;

        if (lifes <= 0)
        {
            //TODO some death effects
            StartCoroutine(Death(0f));
        }
        else
        {
            StartCoroutine(Invinsibility(1.5f));
        }
    }

    private IEnumerator Death(float time)
    {
        coins = Mathf.RoundToInt(coins * (1 - coinLossFactor));
        
        canMove = false;
        yield return new WaitForSeconds(time);
        Time.timeScale = 0;
        playerStats.SetActive(false);
        deathScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(afterDeathButton);
    }

    private IEnumerator Invinsibility(float time)
    {
        canGetDamage = false;
        float timeOver = 0f;

        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);

        yield return new WaitForSeconds(.2f);

        while (timeOver < time-.2f)
        {
            timeOver += Time.deltaTime;

            spriteRenderer.color = Mathf.RoundToInt(timeOver * 6) % 2 == 0? new Color(1f,1f,1f,1f):new Color(1f,1f,1f,.4f);
            
            yield return 0;
        }

        spriteRenderer.color = new Color(1f,1f,1f,1f);
        canGetDamage = true;
    }

    public void Respawn()
    {
        //TODO Respawn alles nötige
        //TODO Teleportiere zum Dorf
        rigidbody.velocity = Vector2.zero;
        
        GameManager.instance.SetNightmare(false);
        
        InitializeStats();
        ToSpawnPoint();
        Time.timeScale = 1;
        canDreamShift = true;
    }

    private void ToSpawnPoint()
    {
        transform.position = SpawnPoint.instance.transform.position;
        rigidbody.velocity = Vector2.zero;
    }

    public void GetDamage(Vector2 knockback)
    {
        if (!canGetDamage) return;
        GetDamage();
        
        movedAfterHit = false;
        rigidbody.velocity =  knockback * attackHitKnockback;

        StartCoroutine(ResetKnockbackAfterTime(knockback.magnitude * attackHitKnockback * 0.043f));
    }

    private void Attack()
    {
        if(!canMove) return;
        if(!canAttack) return;
        
        canAttack = false;
        StartCoroutine(AttackCooldown());

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
        PlayAudio(attackSounds[Random.Range(0,attackSounds.Count)]);
        
        //links rechts oben unten
        //0     1      2    3
        Vector3[] attackPoints = {
            transform.position + Vector3.left * .7f + Vector3.down * .25f,
            transform.position + Vector3.right * .7f + Vector3.down * .25f,
            transform.position + Vector3.up * .2f,
            transform.position + Vector3.down * .4f};
        Vector3 attackPoint = attackPoints[direction];
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint, radius, hittableLayers);
        
        if (enemiesHit.Length == 0) return; //No Hit
        
        Vector2[] knockbackVectors = { Vector2.right, Vector2.left, Vector2.down, Vector2.up };
        Vector2 playerKnockback = knockbackVectors[direction];
        Vector2[] enemyKnockbackVectors = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
        Vector2 enemyKnockback = enemyKnockbackVectors[direction];
        
        movedAfterHit = false;
        foreach (var enemy in enemiesHit)
        {
            if (enemy.CompareTag("Fruit"))
            {
                Fruit fruit = enemy.GetComponent<Fruit>();
                fruit.ObtainFruit();
                continue;
            }
            
            Hittable enemyHealth = enemy.GetComponent<Hittable>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(attackDamage, enemyKnockback * attackHitEnemyKnockback);
            }
        }
        
        //TODO do sfx stuff to indicate hit

        rigidbody.velocity =  playerKnockback* attackHitKnockback * (direction < 2 ? .2f:1f);

        if (direction < 2) StartCoroutine(ResetKnockbackAfterTime(.3f));
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator ResetKnockbackAfterTime(float time)
    {
        float timeOver = 0f;

        while (timeOver < time)
        {
            timeOver += Time.deltaTime;
            if(movedAfterHit) yield break;
            yield return 0;
        }
        
        CheckForMove();
    }

    private void Interact()
    {
        if (abilityGranted)
        {
            abilityGranted = false;
            abilityScreen.gameObject.SetActive(false);
            Time.timeScale = 1;
            canMove = true;
            playerStats.SetActive(true);
            return;
        }
        
        if (dialogueCoroutine != null)
        {
            if(dialogueState == 1)
            {
                dialogueState = 2;
                return;
            }

            if (dialogueState == 2)
            {
                dialogueState = 1;
                return;
            }
            if(dialogueState == 3)
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

    public void ObtainCoins(int amount)
    {
        //TODO effect stuff on obtaining coins
        coins += amount;
        SetUICoins();
    }

    public void ObtainEssence(float amount)
    {
        //TODO effect stuff on obtaining essence
        dreamEssence += amount;
        if (dreamEssence > essenceCapacity) dreamEssence = essenceCapacity;
        SetUIEssenzBar();
    }

    public void SetCanDreamShift(bool canShift)
    {
        this.canDreamShift = canShift;
    }
    
    private void DreamShift()
    {
        if(!canDreamShift) return;
        if(!canMove) return;
        //if(!isGrounded) return;

        if (godMode)
        {
            GameManager.instance.SwitchNightmare();
            Instantiate(dreamShiftPS, transform);
        }
        else if (dreamEssence >= 1f)
        {
            dreamEssence -= 1f;
            SetUIEssenzBar();

            if (GameManager.instance.nightmareMode && hasGrappling == 1)
            {
                if (currentTarget != null)
                {
                    currentTarget.Untarget();
                    currentTarget = null;
                }
            }
            
            GameManager.instance.SwitchNightmare();
            Instantiate(dreamShiftPS, transform);
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

    private bool CanUseAbility(AbilityType abilityType)
    {
        if (godMode) return true;
        switch (abilityType)
        {
            case AbilityType.Grappling:
                if (hasGrappling == 1 && GameManager.instance.nightmareMode) return true;
                if (hasGrappling == 2) return true;
                break;
            case AbilityType.DoubleJump:
                if (hasDoubleJump == 1 && GameManager.instance.nightmareMode) return true;
                if (hasDoubleJump == 2) return true;
                break;
            case AbilityType.Gliding:
                if (hasGlide == 1 && GameManager.instance.nightmareMode) return true;
                if (hasGlide == 2) return true;
                break;
        }

        return false;
    }

    private void Glide(bool pressed)
    {
        if (isGrounded && pressed && canDash)
        {
            StartCoroutine(Dash());
        }
        
        if(!CanUseAbility(AbilityType.Gliding)) return;
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

    private IEnumerator Dash()
    {
        canMove = false;
        canDash = false;
        canGetDamage = false;
        rigidbody.gravityScale = 0;

        (transform.localScale.x > 0 ? dashPSRight : dashPSLeft).SetActive(true);
        
        movementLocked = true;
        float time = 0f;

        while (time < dashTime)
        {
            time += Time.deltaTime;
            rigidbody.velocity = Vector2.right * (transform.localScale.x * dashSpeed);
            yield return 0;
        }
        
        movementLocked = false;
        
        canMove = true;
        canGetDamage = true;
        rigidbody.gravityScale = 1;
        rigidbody.velocity = Vector2.zero;
        
        dashPSLeft.SetActive(false);
        dashPSRight.SetActive(false);
        
        CheckForMove();

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void Grappling()
    {
        if(!CanUseAbility(AbilityType.Grappling)) return;
        if(!canMove) return;
        if (movementLocked) return;
        if (currentTarget == null) return;

        StartCoroutine(UseGrappling());
    }

    private IEnumerator UseGrappling()
    {
        Grappable target = currentTarget;
        
        CancelGliding();
        movementLocked = true;
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
        movementLocked = false;
        canMove = true;
        
        if (playerInput.Movement.Move.IsPressed())
        {
            CheckForMove();
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
        
        if (other.CompareTag("EssenceTrigger"))
        {
            other.GetComponent<Essence>().Obtain();
        }

        if (other.CompareTag("Essence"))
        {
            ObtainEssence(other.GetComponent<Essence>().essenceAmount);
            Destroy(other.transform.parent.gameObject);
        }
        
        if (other.CompareTag("CoinTrigger"))
        {
            other.GetComponent<Coin>().Obtain();
        }

        if (other.CompareTag("Coin"))
        {
            ObtainCoins(1);
            Destroy(other.transform.parent.gameObject);
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

    public void StartDialogue(List<string> sentences)
    {
        dialogueCoroutine = StartCoroutine(Writer(sentences));
    }
    
    private IEnumerator Writer(List<string> sentences)
    {
        dialogueText.text = "";
        dialogueObject.SetActive(true);
        canMove = false;
        playerStats.SetActive(false);

        for (var i = 0; i < sentences.Count; i++)
        {
            var sentence = sentences[i];
            dialogueText.text = "";
            dialogueState = 1;

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

            if (i >= sentences.Count -1) break;

            while (dialogueState == 2)
            {
                yield return 0;
            }
        }

        dialogueState = 3;
    }

    public void UnlockAbility(Sprite icon, string title, string description, AbilityType abilityType)
    {
        //TODO SFX Stuff and sounds

        Time.timeScale = 0;
        
        abilityScreen.SetUpScreen(icon,title,description);
        abilityScreen.gameObject.SetActive(true);
        abilityGranted = true;
        canMove = false;
        playerStats.SetActive(false);
        

        switch (abilityType)
        {
            case AbilityType.Health:
                maxLifes++;
                lifes = maxLifes;
                SetUILives();
                break;
            case AbilityType.Damage:
                attackDamage++;
                break;
            case AbilityType.Grappling:
                hasGrappling++;
                break;
            case AbilityType.DoubleJump:
                hasDoubleJump++;
                break;
            case AbilityType.Gliding:
                hasGlide++;
                break;
        }
    }

    public enum AbilityType
    {
        Health,
        Damage,
        Grappling,
        DoubleJump,
        Gliding
    }

    private void PlayAudio(AudioClip clip)
    {
        Instantiate(audioObject,transform).GetComponent<AudioObject>().Initialize(clip);
    }

    private void RecallStart()
    {
        if (!canMove) return;
        if (!isGrounded) return;

        recallCoroutine = StartCoroutine(Recall());
    }

    private IEnumerator Recall()
    {
        canMove = false;
        
        //TODO some particle effect on recall
        
        yield return new WaitForSeconds(recallTime);
        
        canMove = true;
        
        recallCoroutine = null;
        Respawn();
    }

    private void CancelRecall()
    {
        if(recallCoroutine != null)
        {
            StopCoroutine(recallCoroutine);
            canMove = true;
        }
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

    private void SetUICoins()
    {
        coinsText.text = coins.ToString();
    }
}
