using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossroomManager : MonoBehaviour
{
    public bool fightActive = false;
    public CinemachineVirtualCamera virtualCamera;
    void Entry()
    {
        fightActive = true;
        virtualCamera.Priority = 15;
    }
    void Exit()
    {
        fightActive = true;
        virtualCamera.Priority = 5;
    }

    public void SwitchFightActive()
    {
        if (fightActive) Exit();
        else Entry();
    }
}
