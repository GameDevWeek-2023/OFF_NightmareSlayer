using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : Hittable
{
    public int health;
    [HideInInspector] public int startHealth;
    public bool blinkOnDamage = true;
    public bool kill = false;
    public UnityEvent onDeath;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        startHealth=health;
    }

    private void Update()
    {
        if (kill)
        {
            Damage(health);
            kill = false;
        }
    }

    public override void Damage(int amount)
    {
        if(blinkOnDamage) Blink();
        health -= amount;
        onGetDamage.Invoke();
        if(health <= 0)
        {
            onDeath.Invoke();
            PlayHitAudio();
        }
    }

    public override void Damage(int amount, Vector2 direction)
    {
        if(blinkOnDamage) Blink();
        health -= amount;
        onGetDamage.Invoke();
        if(rb!=null)
            rb.velocity = direction;
        if (health <= 0)
        {
            onDeath.Invoke();
            PlayHitAudio();
        }
    }

    private void PlayHitAudio()
    {
        var audioClips = PlayerScript.instance.hitSounds;
        PlayerScript.instance.PlayAudio(audioClips[Random.Range(0,audioClips.Count)]);
    }

    public void Blink()
    {
        if(spriteRenderer == null) return;

        StartCoroutine(IBlink(.2f));
    }

    private IEnumerator IBlink(float time)
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(time);
        spriteRenderer.color = new Color(1f,1f,1f,1f);
    }
}
