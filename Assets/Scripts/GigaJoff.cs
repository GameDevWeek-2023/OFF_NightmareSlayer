using System.Collections.Generic;
using UnityEngine;

public class GigaJoff : MonoBehaviour
{
    public List<AudioClip> dialogueAudio;

    private DialogueStarter dialogueStarter;
    
    private List<string> dialogue1;
    private List<string> dialogue2;

    private void Awake()
    {
        dialogueStarter = GetComponent<DialogueStarter>();

        dialogue1 = new List<string>
        {
            "*Ribbit*",
            "Gigajoff\'s the name.",
            "*Ribbit*",
            "Any chance you\'re here to slay nightmares?",
            "*Ribbit*",
            "My left hand...",
            "THE NIGHTMARE!!!",
            "*Ribbit*",
            "Please avenge me...",
            "...far in the woods.",
            "*Ribbit*"
        };

        dialogue2 = new List<string>
        {
            "*Ribbit*",
            "Thanks for everything...",
            "*Ribbit*",
            "You have annihilated my nightmare.",
            "*Ribbit*",
            "Now I can search for my missing brother Jeff in peace...",
            "...The last time I saw him, he was hopping towards the big slope in the mushroom cliffs.",
            "*Ribbit*",
            "Hopefully he won't be stupid enough to try and climb it on his own..."
        };
    }

    public void Talk()
    {
        List<string> text = GameManager.instance.frogSlain ? dialogue2 : dialogue1;
        
        dialogueStarter.StartDialogue(text);
    }
}
