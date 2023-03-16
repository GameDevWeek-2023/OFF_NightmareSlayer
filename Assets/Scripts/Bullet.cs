using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=3;
    private void Update()
    {
        transform.position += transform.right*Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript.instance.GetDamage((PlayerScript.instance.transform.position - transform.position).normalized * .5f);
            Destroy(gameObject);
        }
    }
    
    private float timeToDestroy = 10f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
