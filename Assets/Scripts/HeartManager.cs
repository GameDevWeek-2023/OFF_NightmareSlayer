using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public GameObject heartStart;
    public GameObject heartEnd;
    public GameObject heartPrefab;
    public GameObject brokenPrefab;

    public void SetHearts(int hearts, int max)
    {
        GameObject[] children = new GameObject[transform.childCount];
            
        for (var i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
        
        foreach (var child in children)
        {
            Destroy(child);
        }
        
        Instantiate(heartStart, transform);

        for (int i = 0; i < hearts; i++)
        {
            Instantiate(heartPrefab, transform);
        }

        for (int i = 0; i < max - hearts; i++)
        {
            Instantiate(brokenPrefab, transform);
        }
        
        Instantiate(heartEnd, transform);
    }
}
