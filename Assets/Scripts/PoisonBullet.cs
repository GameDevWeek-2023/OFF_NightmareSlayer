using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    private float timeToDestroy = 3f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
