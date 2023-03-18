using System.Collections;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        StartCoroutine(IInitialize(audioClip.length));
    }
    
    public void Initialize(AudioClip audioClip, float volume)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        StartCoroutine(IInitialize(audioClip.length));
    }

    private IEnumerator IInitialize(float time)
    {
        audioSource.Play();
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
