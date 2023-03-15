using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class FrogTongueAttack : BossAttack
{
    public SpriteSkin spriteSkin;
    public HingeJoint2D bone1Joint;

    public override void Attack()
    {
        spriteSkin.enabled = true;
        bone1Joint.useMotor = true;
        Invoke("StopMotion", 5);
    }
    private void StopMotion()
    {
        bone1Joint.useMotor = false;
        Invoke("DisableSpriteSkin", 3);
    }
    private void DisableSpriteSkin()
    {
        spriteSkin.enabled = false;
        Finished();
    }
}
