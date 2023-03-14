using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool nightmareMode = false;

    private void Awake()
    {
        instance = this;
    }

    public void SetNightmare(bool nightmareMode)
    {
        this.nightmareMode=nightmareMode;
    }
}
