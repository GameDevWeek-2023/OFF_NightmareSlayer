using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("Master Volume", Mathf.Log10(PlayerPrefs.GetFloat("master", 1)) * 20); 
        mixer.SetFloat("Music Volume", Mathf.Log10(PlayerPrefs.GetFloat("music", 1)) * 20); 
        mixer.SetFloat("SFX Volume", Mathf.Log10(PlayerPrefs.GetFloat("sfx", 1)) * 20); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
