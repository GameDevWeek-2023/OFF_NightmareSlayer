using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (CompareTag("Player"))
        {
            PlayerScript.instance.SetCanDreamShift(false);
            GameManager.instance.SetNightmare(false);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (CompareTag("Player"))
        {
            PlayerScript.instance.SetCanDreamShift(true);
        }
    }
}
