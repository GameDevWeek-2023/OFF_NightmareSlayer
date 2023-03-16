using UnityEngine;

public class AbilityUnlocker : MonoBehaviour
{
    public Sprite icon;
    public string title;
    public string description;
    public PlayerScript.AbilityType abilityType;

    public void Unlock()
    {
        PlayerScript.instance.UnlockAbility(icon,title,description,abilityType);
    }
}
