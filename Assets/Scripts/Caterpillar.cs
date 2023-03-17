using System.Collections;
using UnityEngine;

public class Caterpillar : MonoBehaviour
{
    public Sprite Idle;
    public Sprite Compressed;

    public bool canFly;
    public LayerMask ground;
    
    private SpriteRenderer spriteRenderer;
    private bool idle;
    private Rigidbody2D rigidbody;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        idle = true;
    }

    private void Start()
    {
        rigidbody.gravityScale = canFly ? 0f : 1f;
        StartCoroutine(canFly? MovementFly():MovementGround());
    }

    private IEnumerator MovementGround()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            SwitchSprite();

            if (!Physics2D.Raycast(transform.position + Vector3.left * .7f * transform.localScale.x + Vector3.down * .2f,Vector2.down,.5f,ground))
                transform.localScale = new Vector3(-transform.localScale.x,1f,1f);

            float xVel = -1 * transform.localScale.x * 2f;
            
            float time = 0f;
            while(time < .25f)
            {
                time += Time.deltaTime;
                rigidbody.velocity = new Vector2(xVel, rigidbody.velocity.y);
                yield return 0;
            }
            
            rigidbody.velocity = new Vector2(0f,rigidbody.velocity.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * .7f * transform.localScale.x + Vector3.down * .2f, transform.position + Vector3.left * .7f * transform.localScale.x + Vector3.down * .7f);
    }

    private IEnumerator MovementFly()
    {
        while (true)
        {
            yield return new WaitForSeconds(.125f);
            SwitchSprite();
            
            yield return new WaitForSeconds(.125f);
            SwitchSprite();
            
            float time = 0f;
            Vector2 randomVector = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized * 5f;
            while(time < .2f)
            {
                time += Time.deltaTime;
                rigidbody.velocity = randomVector;
                yield return 0;
            }
            
            rigidbody.velocity = new Vector2(0f,0f);
        }
    }

    private void SwitchSprite()
    {
        idle = !idle;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (idle) spriteRenderer.sprite = Idle;
        else spriteRenderer.sprite = Compressed;
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}
