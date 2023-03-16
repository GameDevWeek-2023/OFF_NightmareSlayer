using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossAttack : MonoBehaviour
{
    protected Boss boss;
    protected void Awake()
    {
        boss=GetComponent<Boss>();
    }

    public bool[] phases;
    public virtual void Attack(){}
    public virtual void AttackUpdate(){}
    protected void Finished()
    {
        boss.AttackFinished();
    }
}
