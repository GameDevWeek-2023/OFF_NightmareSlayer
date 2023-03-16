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
        Close();
    }
    void Exit()
    {
        if (!locked)
        {
            fightActive = true;
            virtualCamera.Priority = 5;
        }
    }

    public void Defeated()
    {
        locked = false;
        Open();
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
}
