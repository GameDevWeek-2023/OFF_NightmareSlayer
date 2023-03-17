using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthSystem))]
public class Boss : MonoBehaviour
{
    public int phaseCount=2;
    public int phaseHealth = 50;
    public float minBreak=1f;
    public float maxBreak=3f;
    public BossroomManager bossroomManager;
    public int currentPhase=0;
    private BossAttack currentAttack;
    private List<BossAttack>[] bossAttacks;
    public bool alive = true;

    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public HealthSystem healthSystem;

    private void Awake()
    {
        bossAttacks = new List<BossAttack>[phaseCount];
        BossAttack[] myBossAttacks = GetComponents<BossAttack>();
        for (int i = 0; i < phaseCount; i++)
        {
            bossAttacks[i] = new List<BossAttack>();
            foreach (BossAttack bossAttack in myBossAttacks)
            {
                if (bossAttack.phases[i])
                {
                    bossAttacks[i].Add(bossAttack);
                }
            }
        }
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        animator.SetInteger("Phase", currentPhase);
    }

    public void StartAttackLoop()
    {
        Attack();
    }

    private void Attack()
    {
        currentAttack = bossAttacks[currentPhase][Random.Range(0, bossAttacks[currentPhase].Count)];
        currentAttack.Attack();
    }

    public void AttackFinished()
    {
        currentAttack = null;
        if(alive)
            Invoke("Attack",Random.Range(minBreak,maxBreak));
    }
    public void OnDamage()
    {
        if (healthSystem.startHealth-healthSystem.health - currentPhase * phaseHealth>=50&&currentPhase<phaseCount-1)
        {
            currentPhase++;
            animator.SetInteger("Phase",currentPhase);
        }
    }

    public void Update()
    {
        if(currentAttack != null)
        {
            currentAttack.AttackUpdate();
        }
    }

    public void Kill()
    {
        alive = false;
        animator.Play("Idle");
    }
}
