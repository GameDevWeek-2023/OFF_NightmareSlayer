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
        normal.SetActive(true);
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

    public static void SwitchAll(bool isNightmare)
    {
        if (allSwitchables == null) return;
        foreach (var switchableObject in allSwitchables)
        {
            switchableObject.SwitchObject(isNightmare);
        }
    }
}
