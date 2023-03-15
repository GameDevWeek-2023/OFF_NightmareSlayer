using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grappable : MonoBehaviour
{
    private Image targetedSprite;

    private void Awake()
    {
        targetedSprite = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        
        targetedSprite.fillAmount = 0f;
    }

    public void Target()
    {
        Untarget();
        StartCoroutine(StartTarget());
    }

    private IEnumerator StartTarget()
    {
        while (targetedSprite.fillAmount < 1f)
        {
            targetedSprite.fillAmount += Time.deltaTime / 0.18f;
            yield return 0;
        }
        targetedSprite.fillAmount = 1f;
    }

    public void Untarget()
    {
        StopAllCoroutines();
        targetedSprite.fillAmount = 0f;
    }
}
