using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class FrogTongueAttack : BossAttack
{
    public SpriteSkin spriteSkin;
    public HingeJoint2D bone1Joint;
    public Vector3[] bonePositions;
    public Quaternion[] boneRotations;
    public Animator tongueAnimator;
    public ParticleSystem fireParticleSystem;

    public override void Attack()
    {
        spriteSkin.gameObject.SetActive(true);
        Invoke("StartRotation", 1);
        boss.animator.Play("OpenMouth");
    }

    private void StartRotation()
    {
        spriteSkin.enabled = true;
        bone1Joint.useMotor = true;
        Invoke("StopMotion", 5);
        bonePositions = new Vector3[spriteSkin.boneTransforms.Length];
        boneRotations = new Quaternion[spriteSkin.boneTransforms.Length];
        for (int i = 0; i < spriteSkin.boneTransforms.Length; i++)
        {
            Transform t = spriteSkin.boneTransforms[i];
            bonePositions[i] = t.localPosition;
            boneRotations[i] = t.localRotation;
        }
        fireParticleSystem.Play();
    }
    private void StopMotion()
    {
        bone1Joint.useMotor = false;
        Invoke("DisableSpriteSkin", 8);
    }
    private void DisableSpriteSkin()
    {
        for (int i = 0; i < spriteSkin.boneTransforms.Length; i++)
        {
            Transform t = spriteSkin.boneTransforms[i];
            t.localPosition = bonePositions[i];
            t.localRotation = boneRotations[i];
            t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        fireParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        spriteSkin.enabled = false;
        tongueAnimator.Play("Tongue_off");
        Invoke("DisableObject", 2);
    }

    private void DisableObject()
    {
        spriteSkin.gameObject.SetActive(false);
        boss.animator.Play("CloseMouth");
        Invoke("Finished", 1);
    }
}
