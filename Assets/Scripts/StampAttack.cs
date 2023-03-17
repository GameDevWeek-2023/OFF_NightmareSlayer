using UnityEngine;

public class StampAttack : BossAttack
{
    public GameObject shockWave;
    public GameObject weakPoint;
    public override void Attack()
    {
        boss.animator.Play("Stomp");
    }

    //Called by animation
    public void Shockwave()
    {
        Instantiate(shockWave, transform.position + Vector3.up * -3f, Quaternion.identity);
        Instantiate(shockWave, transform.position + Vector3.up * -3f, Quaternion.Euler(0, 180, 0));
        if (boss.currentPhase == 0)
        {
            weakPoint.SetActive(true);
            Invoke("End", 5);
        }
        else
        {
            Invoke("End", .2f);
        }
    }

    public void End()
    {
        boss.animator.Play("Stomp_End");
        weakPoint.SetActive(false);
        Finished();
    }
}
