using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossroomManager : MonoBehaviour
{
    public bool playerInRoom = false;
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
        playerInRoom = true;
        if (!GameManager.instance.frogSlain) { 
            locked = true;
            onEntry.Invoke();
            MusicManager.instance.StartBossFight();
            Close();
        }
        virtualCamera.Priority = 15;
        PlayerScript.instance.SetCanDreamShift(false);
    }
    void Exit()
    {
        if (!locked)
        {
            Debug.Log("Exit");
            playerInRoom = false;
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

        if (playerInRoom) Exit();
        else Entry();
    }

    public void SwitchNightmareMode()
    {
        if (GameManager.instance.nightmareMode)
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
        GameManager.instance.frogSlain = true;
        MusicManager.instance.Restart();
        locked = false;
    }
}
