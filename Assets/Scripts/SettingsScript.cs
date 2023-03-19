using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioMixer mixer;

    private void Awake()
    {
        masterSlider.value = PlayerPrefs.GetFloat("master", 1);
        musicSlider.value = PlayerPrefs.GetFloat("music", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("sfx", 1);
        //OnSliderChange();
    }
    public void back()
    {
        gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) back();
    }

    public void Volume(float sliderValue)
    {
    }

    public void Update()
    {
        mixer.SetFloat("Master Volume", Mathf.Log10(masterSlider.value) * 20);
        mixer.SetFloat("Music Volume", Mathf.Log10(musicSlider.value) * 20);
        mixer.SetFloat("SFX Volume", Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetFloat("master", masterSlider.value);
        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.SetFloat("sfx", sfxSlider.value);
    }
}
