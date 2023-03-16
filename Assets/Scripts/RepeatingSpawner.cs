using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingSpawner : MonoBehaviour
{
    public bool startOnAwake = false;
    public float minWaitTime=5;
    public float maxWaitTime=12;
    public GameObject toSpawn;

    private void Awake()
    {
        if(startOnAwake)
            StartSpawning();
    }

    // Start is called before the first frame update
    public void StartSpawning()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(toSpawn, transform.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }
}
