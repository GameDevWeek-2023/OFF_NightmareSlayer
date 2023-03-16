using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossroomManager : MonoBehaviour
{
    public bool fightActive = false;
    public CinemachineVirtualCamera virtualCamera;
    public bool locked=false;
    public UnityEvent onEntry;
    void Entry()
    {
        fightActive = true;
        onEntry.Invoke();
        virtualCamera.Priority = 15;
        locked = true;
    }
    void Exit()
    {
        if (!locked)
        {
            fightActive = true;
            virtualCamera.Priority = 5;
        }
    }

    public void SwitchFightActive()
    {
        if (fightActive) Exit();
        else Entry();
    }
}
