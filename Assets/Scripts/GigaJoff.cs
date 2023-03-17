using System.Collections.Generic;
using UnityEngine;

public class GigaJoff : MonoBehaviour
{
    private DialogueStarter dialogueStarter;
    
    private List<string> dialogue1;
    private List<string> dialogue2;

    private void Awake()
    {
        dialogueStarter = GetComponent<DialogueStarter>();

        dialogue1 = new List<string> { "" };

        dialogue2 = new List<string> { "" };
    }

    public void Talk()
    {
        List<string> text = GameManager.instance.frogSlain ? dialogue2 : dialogue1;
        
        dialogueStarter.StartDialogue(text);
    }
}
