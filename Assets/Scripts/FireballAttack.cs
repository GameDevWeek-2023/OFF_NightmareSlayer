using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class FireballAttack : BossAttack
{
    public GameObject fireball;
    public Transform fireballSpawn;
    public GameObject container;
    // Start is called before the first frame update

    public override void Attack()
    {
        boss.animator.Play("OpenMouth");
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        for (int i = 0; i < (boss.currentPhase==0?2:4); i++)
        {
            yield return new WaitForSeconds(.4f);
            Instantiate(fireball, fireballSpawn.position, Quaternion.Euler(0, 0, Random.Range(0, 360)),container.transform);
        }
        CloseMouth();
    }


    public void CloseMouth()
    {
        boss.animator.Play("CloseMouth");
        Finished();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
