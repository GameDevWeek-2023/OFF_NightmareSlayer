using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent onTrigger;
    public bool lockWayBack;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTrigger.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (lockWayBack)
                GetComponent<Collider2D>().isTrigger = false;
        }
    }
    public void Unlock()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    protected void OnTrigger() { }
}
