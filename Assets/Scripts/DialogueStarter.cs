using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public List<string> dialogueText;

    public void StartDialogue(List<string> dialogueText)
    {
        PlayerScript.instance.SetDialogueSpeed(0.05f);
        PlayerScript.instance.StartDialogue(dialogueText);
    }
    
    public void StartDialogueWithAttribute()
    {
        PlayerScript.instance.SetDialogueSpeed(0.05f);
        PlayerScript.instance.StartDialogue(dialogueText);
    }
}
