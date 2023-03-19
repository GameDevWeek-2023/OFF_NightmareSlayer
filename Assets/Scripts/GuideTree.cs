using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideTree : MonoBehaviour
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
            "Greetings Nightmare Slayer!",
            "I am the oldest being in this village. An ancient Ent from the old times.",
            "Let me explain to you how you can navigate your way in the forests...",
            "You can move with WASD or the left analog stick.",
            "You can attack with left mouse click or the X button.",
            "When you have enough dream essence (top left corner), you can press Q or the Y Button to switch to nightmare mode.",
            "It will give you new abilities after you unlock them. But be careful: everything is more dangerous in nightmares.",
            "You can jump with space or the A button and dash with shift or the left trigger.",
            "You can hold V or Up on the D-Pad to return to this Village quickly. It will restore your health without the loss of your coins.",
            "Now try climbing on the roof of the water mill there to your right.",
            "Good luck slayer!"
        };

        dialogue2 = new List<string>
        {
            "Greetings Nightmare Slayer!",
            "You have successfully fought your way through the burning woods and defeated the nightmare that has dwelled there for centuries!",
            "I can't teach you much more, but talk to the other villagers.",
            "Everyone has their own fears and challenges that you can help them with.",
            "Good luck nightmare slayer!"
        };
    }

    public void Talk()
    {
        List<string> text = GameManager.instance.frogSlain ? dialogue2 : dialogue1;
        
        dialogueStarter.StartDialogue(text, !GameManager.instance.frogSlain ? dialogueAudio1:dialogueAudio2);
    }
}
