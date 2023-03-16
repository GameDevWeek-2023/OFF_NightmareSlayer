using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool nightmareMode = false;
    
    //Game Progress
    public bool frogSlain = false;
    
    //ONLY FOR DEBUG
    public bool switchNightmare = false;
    public UnityEvent onSwitch;

    private void Awake()
    {
        instance = this;
    }
    
    public void SwitchNightmare()
    {
        nightmareMode = !nightmareMode;
        SwitchableObject.SwitchAll(nightmareMode);
        ReplaceEnemy.ReplaceAll(nightmareMode);
        Fruit.SwitchAllFruit(nightmareMode);
        onSwitch.Invoke();
    }

    public void SetNightmare(bool nightmareMode)
    {
        this.nightmareMode=nightmareMode;
        SwitchableObject.SwitchAll(nightmareMode);
        ReplaceEnemy.ReplaceAll(nightmareMode);
        Fruit.SwitchAllFruit(nightmareMode);
        onSwitch.Invoke();
    }

    private void Update()
    {
        if (switchNightmare)
        {
            SetNightmare(!nightmareMode);
            switchNightmare = false;
        }
    }
}
