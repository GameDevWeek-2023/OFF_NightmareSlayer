using UnityEngine;

public class StampAttack : BossAttack
{
    public GameObject shockWave;
    public override void Attack()
    {
        Instantiate(shockWave, transform.position+Vector3.up * -3f, Quaternion.identity);
        Instantiate(shockWave, transform.position+Vector3.up*-3f, Quaternion.Euler(0, 180, 0));
        
        Invoke("Finished", 5);

    }
}
