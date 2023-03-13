using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UiScript : MonoBehaviour
{
    public Image essenzBar;

    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Image heart4;
    public Image heart5;

    public int essenz; //zwischen 0 und 100
    public int lives = 3;

    private void Update()
    {
        
    }

    public void setEssenz(int plus)
    {
        essenz += plus;
        if (essenz > 100) essenz = 100;
        essenzBar.fillAmount = essenz / 100f;
    }

    public void checkHearts()
    {
        switch (lives)
        {
            case 0: 
                heart1.enabled = false;
                heart2.enabled = false;
                heart3.enabled = false;
                heart4.enabled = false;
                heart5.enabled = false;
                break;
            case 1: 
                heart1.enabled = true;
                heart2.enabled = false;
                heart3.enabled = false;
                heart4.enabled = false;
                heart5.enabled = false;
                break;
            case 2: 
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = false;
                heart4.enabled = false;
                heart5.enabled = false;
                break;
            case 3: 
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = false;
                heart5.enabled = false;
                break;
            case 4: 
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = false;
                break;
            default:
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                break;
        }
    }
}
