using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript.instance.GetDamage((PlayerScript.instance.transform.position-transform.position).normalized*.5f);
        }
    }
}
