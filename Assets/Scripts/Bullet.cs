using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=3;
    public bool bounce;
    public float timeToDestroy = 5f;
    public LayerMask collidingLayers;
    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;

        if (bounce) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f, collidingLayers);
            if (hit)
            {
                Debug.Log("Bounce");
                Vector2.Reflect(transform.right, hit.normal);
                
                transform.rotation =Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right,Vector2.Reflect(transform.right, hit.normal)));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            PlayerScript.instance.GetDamage((PlayerScript.instance.transform.position - transform.position).normalized * .5f);
            
        }
        if (!bounce||collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
