using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=3;
    public bool bounce;
    public float timeToDestroy = 5f;
    private void Update()
    {
        transform.position += transform.right*Time.deltaTime * speed;
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
        else
        {
            Debug.Log("Bounce");
            transform.RotateAround(new Vector3(0, 0, 1), 180);
        }
    }
    

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
