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
        Invoke("Finished", 5);
    }
}