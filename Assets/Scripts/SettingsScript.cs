using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    public void back()
    {
        gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) back();
    }

    public AudioMixer mixer; 
    public void Volume(float sliderValue)
    {
        //PlayerPrefs.SetFloat("volume", sliderValue);
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}
