using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knightshroom : MonoBehaviour
{
    public List<AudioClip> dialogueAudio1;
    public List<AudioClip> dialogueAudio2;
    
    private DialogueStarter dialogueStarter;
    
    private List<string> dialogue1;
    private List<string> dialogue2;

    private void Awake()
    {
        dialogueStarter = GetComponent<DialogueStarter>();

        dialogue1 = new List<string>
        {
            "Greetings noble knight, my name is Knightshroom.",
            "I am pleased to make your acquaintance.",
            "But be warned noble knight.",
            "You should not take the deadly shroomcliffs so lightly on your shoulders.",
            "In the service of my king, I have dropped countless companions in this place.",
            "Therefore, for the time being, turn your attention to the burning woods."
        };

        dialogue2 = new List<string>
        {
            "My deepest gratefulness, noble knight.",
            "Now my children can play safely in the woods again without fear of nightmares.",
            "However, I still cannot take up my service to the king as long as the shroomcliffs are still plagued by nightmares.",
            "I know it is not proper for such a noble and code-bound knight as myself, but I hereby ask you, oh great nightmare slayer...",
            "...free my homeland, the shroomcliffs, and I will reward you richly.",
            "I would come help you, but I am waiting for my left hand to grow back...",
            "...so that I may have enough strength to swing my mighty sword once again..."
        };
    }

    public void Talk()
    {
        List<string> text = GameManager.instance.frogSlain ? dialogue2 : dialogue1;
        
        dialogueStarter.StartDialogue(text, !GameManager.instance.frogSlain ? dialogueAudio1:dialogueAudio2);
    }
}
