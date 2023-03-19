using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MawPlant : MonoBehaviour
{
    public bool isNightmare;
    public float attackDelay = 2f;
    public GameObject projectile;
    private Animator animator;
    private Collider2D collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();

        if(collider != null) collider.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(isNightmare?AttackN():AttackNO());
    }

    private IEnumerator AttackNO()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);
            
            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(.45f);
            collider.enabled = true;
            yield return new WaitForSeconds(.1f);
            collider.enabled = false;
            yield return new WaitForSeconds(.35f);
        }
    }
    
    private IEnumerator AttackN()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);
            
            animator.SetTrigger("Attack");
            
            yield return new WaitForSeconds(.35f);
            Instantiate(projectile, transform.position + transform.rotation*new Vector3(-0.663f,0.24f,0f),
                transform.rotation,transform);
            yield return new WaitForSeconds(.75f);
        }
    }
}
