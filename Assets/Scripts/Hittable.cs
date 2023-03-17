using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{

    public UnityEvent onGetDamage;
    public virtual void Damage(int amount)
    { }

    public virtual void Damage(int amount, Vector2 direction)
    { }
}
