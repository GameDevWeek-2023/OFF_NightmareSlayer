using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappable : MonoBehaviour
{
    private GameObject targetedSprite;

    private void Awake()
    {
        targetedSprite = transform.GetChild(0).gameObject;
        
        targetedSprite.SetActive(false);
    }

    public void Target()
    {
        targetedSprite.SetActive(true);
    }

    public void Untarget()
    {
        targetedSprite.SetActive(false);
    }
}
