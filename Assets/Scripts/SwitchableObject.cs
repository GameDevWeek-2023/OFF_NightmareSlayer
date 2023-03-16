using System.Collections.Generic;
using UnityEngine;

public class SwitchableObject : MonoBehaviour
{
    public GameObject normal;
    public GameObject nightmareMode;

    private static List<SwitchableObject> allSwitchables;

    private void Awake()
    {
        if (allSwitchables == null) allSwitchables = new List<SwitchableObject>();
        if (!allSwitchables.Contains(this)) allSwitchables.Add(this);
    }

    void Start()
    {
        nightmareMode.SetActive(false);
    }

    public void SwitchObject(bool isNightmare)
    {
        if (isNightmare)
        {
            normal.SetActive(false);
            nightmareMode.SetActive(true);
        }
        else
        {
            normal.SetActive(true);
            nightmareMode.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        allSwitchables.Remove(this);
    }

    public static void SwitchAll(bool isNightmare)
    {
        if (allSwitchables == null) return;
        for (int i = allSwitchables.Count-1; i >= 0; i--)
        {
            SwitchableObject switchable = allSwitchables[i];
            switchable.SwitchObject(isNightmare);
        }
    }
}
