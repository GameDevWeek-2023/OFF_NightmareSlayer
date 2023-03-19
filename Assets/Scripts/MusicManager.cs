using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public TrackPair[] tracks;
    public float fadeDuration;
    public float musicVolume;
    public AudioSource source1;
    public AudioSource source2;
    public AudioClip bossfightStart;
    public AudioClip bossfightLoop;
    private float currentFade;
    private bool source1Active=false;
    private int currentTrack;
    private void Awake()
    {
        source1Active = GameManager.instance.nightmareMode;
        instance = this;
        (source1Active ? source1 : source2).volume = 0;
        (!source1Active ? source1 : source2).volume = musicVolume;
        source1.clip = tracks[currentTrack].normal;
        source2.clip = tracks[currentTrack].nightmareMode;
        source1.Play();
        source2.Play();
    }

    private void Update()
    {
        if (currentFade > 0)
        {
            currentFade -= Time.deltaTime;
            float fade = fadeDuration * currentFade;
            (source1Active ? source1 : source2).volume = musicVolume * fade;
            (!source1Active ? source1 : source2).volume = musicVolume * (1f - fade);
        }
    }

    public void StartFade()
    {
        currentFade = fadeDuration;
        source1Active = GameManager.instance.nightmareMode;
    }

    public void StartFade(float fadeDuration)
    {
        currentFade = fadeDuration;
    }

    public void Pause()
    {
        (!source1Active ? source1 : source2).volume = 0;
    }

    public void Resume()
    {
        (!source1Active ? source1 : source2).volume = musicVolume;
    }

    public void StartBossFight()
    {
        source1.clip = bossfightStart;
        source1.volume=musicVolume;
        source1.Play();
        source2.volume = 0;
        currentFade = 0;
        Invoke("BossFightLoop", bossfightStart.length);
    }
    public void BossFightLoop()
    {
        source1.clip = bossfightLoop;
        source1.Play();
    }

    public void Restart()
    {
        CancelInvoke();
        Awake();
    }

}
