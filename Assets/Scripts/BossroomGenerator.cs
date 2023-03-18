using UnityEngine;

public class BossroomGenerator : MonoBehaviour
{
    public GameObject bossroom;
    private GameObject currentBossroom;
    public static BossroomGenerator instance;
    private void Awake()
    {
        currentBossroom=Instantiate(bossroom);
        instance=this;
    }

    public void Reload()
    {
        Destroy(currentBossroom);
        currentBossroom=Instantiate(bossroom);
    }

    public void SwitchNightmareMode()
    {
        currentBossroom.GetComponent<BossroomManager>().SwitchNightmareMode();
    }
}
