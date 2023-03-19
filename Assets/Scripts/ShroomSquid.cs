using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomSquid : MonoBehaviour
{
    public bool isSquid;
    public LayerMask ground;
    
    private Animator animator;
    private Rigidbody2D rigidbody;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(isSquid? MovementSquid():MovementGround());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(!isSquid)
        {
            Gizmos.DrawLine(transform.position + Vector3.down * .4f,
                transform.position + Vector3.down * 1f);
        }
    }

    private IEnumerator MovementGround()
    {
        float movementSpeed = 5f;
        float movementDuration = .1f;
        
        while (true)
        {
            animator.enabled = 
                Physics2D.Raycast(transform.position + Vector3.down * .4f,
                    Vector2.down, .6f,ground);
            yield return 0;
        }
    }

    private IEnumerator MovementSquid()
    {
        float movementMinSpeed = 1.3f;
        float movementMaxSpeed = 2f;
        float frictionDuration = 1.5f;
        
        yield return new WaitForSeconds(.1f);
        
        while (true)
        {
            Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            float mag = Random.Range(movementMinSpeed,movementMaxSpeed);
            rigidbody.velocity = dir * mag;
            yield return 0;
            while (mag > .4f)
            {
                mag -= Time.deltaTime / frictionDuration;
                if(mag <= .4f) break;
                dir = dir.normalized * mag;
                rigidbody.velocity = dir * mag;
                yield return 0;
            }
        }
    }
}
