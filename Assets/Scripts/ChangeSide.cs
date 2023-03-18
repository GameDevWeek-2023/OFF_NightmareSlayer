using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSide : BossAttack
{
    private bool isLeft;
    public override void Attack()
    {
        isLeft = !isLeft;
        boss.containerAnimator.SetBool("isLeft",isLeft);
        boss.animator.Play("Hop");
        Invoke("Finished", 5);
    }
}