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
    private Animator animator;
    private bool hidden = true;
    public GameObject bossUtilityContainer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Entry()
    {
        fightActive = true;
        onEntry.Invoke();
        virtualCamera.Priority = 15;
        locked = true;
        PlayerScript.instance.SetCanDreamShift(false);
        Close();
    }
    void Exit()
    {
        if (!locked)
        {
            fightActive = false;
            PlayerScript.instance.SetCanDreamShift(true);
            virtualCamera.Priority = 5;
        }
    }


    public void CameraShake(float duration)
    {
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 5;
        Invoke("StopShake", duration);
    }

    private void StopShake()
    {
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    public void SwitchFightActive()
    {
        if (fightActive) Exit();
        else Entry();
    }

    public void SwitchNightmareMode()
    {
        if (hidden)
            Show();
        else
            Hide();
    }

    public void Show()
    {
        animator.Play("Show");
        hidden = false;
    }

    public void Hide()
    {
        animator.Play("Hide");
        hidden = true;
    }

    public void Open()
    {
        animator.Play("Open");
    }

    public void Close()
    {
        animator.Play("Close");
    }

    public void BossRIP()
    {
        Open();
        foreach (Transform t in bossUtilityContainer.transform) {
            GameObject.Destroy(t.gameObject);
        }
        locked = false;
    }
}
