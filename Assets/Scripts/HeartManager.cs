using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public GameObject brokenPrefab;

    public void SetHearts(int hearts, int max)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0));
        }

        for (int i = 0; i < hearts; i++)
        {
            Instantiate(heartPrefab, transform);
        }

        for (int i = 0; i < max - hearts; i++)
        {
            Instantiate(brokenPrefab, transform);
        }
    }
}
