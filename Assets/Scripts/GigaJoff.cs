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

        dialogue1 = new List<string> { "*Quark*",
            "Any chance you\'re here to slain nightmares?",
            "*Quark*",
            "My left hand...",
            "THE NIGHTMARE!!!",
            "*Quark*",
            "Please avenge me...",
            "...deep in the woods.",
            "*Quark*" };

        dialogue2 = new List<string> { "*Quark*",
            "Thanks for everything...",
            "*Quark*",
            "You have annihilated my nightmare.",
            "*Quark*",
            "Now I can search for my missing brother Jeff in peace...",
            "...The last time I saw him, he was hopping towards the big slope in the mushroom cliffs.",
            "*Quark*",
            "Hopefully he won't be stupid enough to try and climb it on his own..." };
    }

    public void Talk()
    {
        List<string> text = GameManager.instance.frogSlain ? dialogue2 : dialogue1;
        
        dialogueStarter.StartDialogue(text);
    }
}
