using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableObject : MonoBehaviour
{
    public GameObject normal;
    public GameObject nightmareMode;
    // Start is called before the first frame update
    void Start()
    {
        nightmareMode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
