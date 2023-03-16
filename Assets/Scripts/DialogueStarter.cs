using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public void StartDialogue(string dialogueText)
    {
        PlayerScript.instance.SetDialogueSpeed(0.05f);
        PlayerScript.instance.StartDialogue(dialogueText);
    }
}
