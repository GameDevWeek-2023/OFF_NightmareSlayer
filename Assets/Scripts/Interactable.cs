using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent interactionEvent;
    private GameObject interactionText;

    private void Awake()
    {
        interactionText = transform.GetChild(0).gameObject;
        
        HideText();
    }

    public void ShowText()
    {
        interactionText.SetActive(true);
    }

    public void HideText()
    {
        interactionText.SetActive(false);
    }

    public void Interact()
    {
        interactionEvent.Invoke();
    }
}
