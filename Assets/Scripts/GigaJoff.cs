using UnityEngine;

public class GigaJoff : MonoBehaviour
{
    private DialogueStarter dialogueStarter;
    
    private const string dialogue1 = "";
    private const string dialogue2 = "";

    private void Awake()
    {
        dialogueStarter = GetComponent<DialogueStarter>();
    }

    public void Talk()
    {
        string text = GameManager.instance.frogSlain ? dialogue2 : dialogue1;
        
        dialogueStarter.StartDialogue(text);
    }
}
