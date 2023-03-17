using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public bool affectsEnemies = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("ContactDamage");
            PlayerScript.instance.GetDamage((PlayerScript.instance.transform.position-transform.position).normalized*.5f);
        }
        HealthSystem hs = collision.GetComponent<HealthSystem>();
        if (affectsEnemies && hs!=null)
        {
            hs.Damage(1);
        }
    }
}
