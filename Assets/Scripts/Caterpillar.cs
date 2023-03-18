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
        float movementSpeed = 5f;
        float movementDuration = .1f;
        
        while (true)
        {
            while (!Physics2D.Raycast(transform.position + Vector3.down * .1f, Vector2.down, .3f,ground)) yield return 0;
            yield return new WaitForSeconds(.5f);
            SwitchSprite();

            if (!Physics2D.Raycast(transform.position + Vector3.left * .7f * transform.localScale.x + Vector3.down * .1f,Vector2.down,.3f,ground)
                || Physics2D.Raycast(transform.position + Vector3.down * .1f,Vector2.left * transform.localScale.x, .7f,ground))
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
                //Debug.Log("Flip");
            }

            float xVel = -1 * transform.localScale.x * movementSpeed;
            
            float time = 0f;
            while(time < movementDuration)
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
        if(!canFly)
        {
            Gizmos.DrawLine(transform.position + Vector3.left * .7f * transform.localScale.x + Vector3.down * .1f,
                transform.position + Vector3.left * .7f * transform.localScale.x + Vector3.down * .4f);
            Gizmos.DrawLine(transform.position + Vector3.down * .1f,
                transform.position + Vector3.down * .1f + Vector3.left * transform.localScale.x * .7f);
            Gizmos.DrawLine(transform.position + Vector3.down * .1f,
                transform.position + Vector3.down * .4f);
        }
    }

    private IEnumerator MovementFly()
    {
        float movementSpeed = 4f;
        float movementDuration = .08f;
        
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            SwitchSprite();
            
            float time = 0f;
            Vector2 randomVector = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized * movementSpeed;

            spriteRenderer.flipX = randomVector.x < 0;
            
            while(time < movementDuration / 2)
            {
                time += Time.deltaTime;
                rigidbody.velocity = randomVector;
                yield return 0;
            }
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
