public class StampAttack : BossAttack
{
    public override void Attack()
    {
        Invoke("Finished", 5);

    }
}
