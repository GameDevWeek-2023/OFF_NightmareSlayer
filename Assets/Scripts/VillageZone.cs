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
            if (GameManager.instance.nightmareMode)
            {
                GameManager.instance.SwitchNightmare();
            }
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
