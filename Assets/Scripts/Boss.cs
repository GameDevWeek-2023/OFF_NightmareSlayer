using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss : MonoBehaviour
{
    public int phaseCount=1;
    public float minBreak=1f;
    public float maxBreak=3f;
    public BossroomManager bossroomManager;
    private int currentPhase=0;
    private BossAttack currentAttack;
    private List<BossAttack>[] bossAttacks;
    [HideInInspector]
    public Rigidbody2D rb2d;
    public Animator animator;

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
        animator.Play("Idle");
        Invoke("Attack",Random.Range(minBreak,maxBreak));
    }
    void Start()
    {
        
    }

    public void Update()
    {
        if(currentAttack != null)
        {
            currentAttack.AttackUpdate();
        }
    }
}
