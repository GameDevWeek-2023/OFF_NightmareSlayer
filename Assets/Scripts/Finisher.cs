using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Finisher : MonoBehaviour
{
    public static Finisher instance;
    
    public VideoPlayer finisherVideo;
    public GameObject finisherTexture;
    public GameObject doubleJump;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        finisherTexture.SetActive(false);
    }

    public void Finish()
    {
        StartCoroutine(IFinish());
    }

    private IEnumerator IFinish()
    {
        PlayerScript.instance.canMove = false;
        MusicManager.instance.Pause();
        finisherTexture.SetActive(true);
        finisherVideo.Play();
        
        yield return new WaitForSeconds(11.88f);
        
        finisherTexture.SetActive(false);
        PlayerScript.instance.Respawn();
        PlayerScript.instance.canMove = false;
        MusicManager.instance.Resume();
        doubleJump.SetActive(true);
    }
}
