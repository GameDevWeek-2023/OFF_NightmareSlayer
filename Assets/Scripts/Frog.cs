using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Frog : MonoBehaviour
{
    public bool flipped;
    public LayerMask ground;
    public GameObject bullet;
    public GameObject bulletSpawner;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    public float jumpStrength = 10f;
    public float jumpStrengthSide = 10f;
    public float jumpDelta = 3f;
    public bool shooting;
    private float jumpCoolDown = -1;

    public Sprite jump;
    public Sprite idle;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(shooting)
            InvokeRepeating("Shoot",2,2);
    }

    void Shoot()
    {
        Instantiate(bullet, bulletSpawner.transform.position, flipped?Quaternion.Euler(0, 180, 0):Quaternion.identity);
        //Debug.Log("Shoot");
    }

    // Update is called once per frame
    public void Update()
    {
        if (jumpCoolDown == -1)
        {
            jumpCoolDown = jumpDelta * Random.Range(.5f,1.5f);
        }
        else if (jumpCoolDown < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector3.down, 1f, ground))
            {
                Jump();
                jumpCoolDown = -1;
            }
        }
        else
        {
            jumpCoolDown -= Time.deltaTime;
        }

        if (Random.Range(0f,1f) <= .3f)
        {
            flipped = !flipped;
            spriteRenderer.flipX = !flipped;
        }

    }

    private void Jump()
    {
        spriteRenderer.sprite = jump;
        float sideVelocity = 0;
        if (!flipped)
        {
            sideVelocity = jumpStrengthSide;
        }
        else
        {
            sideVelocity = -jumpStrengthSide;
        }

        rb2d.velocity = new Vector2(sideVelocity * Random.Range(.5f, 1.5f), jumpStrength * Random.Range(.5f, 1.5f));
        StartCoroutine(SpriteWait());
    }

    IEnumerator SpriteWait()
    {
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = idle;
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
