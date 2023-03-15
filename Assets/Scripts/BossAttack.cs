using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossAttack : MonoBehaviour
{
    public bool[] phases;
    public void Attack(){}
    public void AttackUpdate(){}
    protected void Finished()
    {
        GetComponent<Boss>().AttackFinished();
    }
}
