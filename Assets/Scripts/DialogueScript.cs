using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string sentence;
    private bool Tlocker = true;

    public float dialogueSpeed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.T) && Tlocker)
        {
            Tlocker = false;
            playSentences();
        }

        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("Skipped");
            skip();
            Tlocker = true;
        }
    }

    public void playSentences()
    {
        StartCoroutine(writer());
    }

    public void skip()
    {
        StopAllCoroutines();
        dialogueText.text = sentence;
    }
    
    IEnumerator writer()
    {
        dialogueText.text = "";
        dialogueText.text = "";
        foreach (char Character in sentence) 
        {
            dialogueText.text += Character; 
            yield return new WaitForSeconds(dialogueSpeed);
        }
        Tlocker = true;
    }
}
