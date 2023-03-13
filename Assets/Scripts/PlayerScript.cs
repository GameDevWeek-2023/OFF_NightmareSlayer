using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Input
    private PlayerInput playerInput;
    private bool canMove;
    //DreamShift
    private bool canDreamShift;
    //Abilities
    private bool hasGlide;
    private bool hasDoubleJump;
    
    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Movement.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Movement.Move.canceled += ctx => Move(ctx.ReadValue<Vector2>());
        playerInput.Movement.Jump.performed += ctx => Jump();
        playerInput.Movement.Attack.performed += ctx => Attack();
        playerInput.Movement.Interact.performed += ctx => Interact();
        playerInput.Movement.DreamShift.performed += ctx => DreamShift();
        playerInput.Movement.Glide.performed += ctx => Glide(true);
        playerInput.Movement.Glide.canceled += ctx => Glide(false);

        playerInput.Movement.Enable();
    }

    private void Move(Vector2 value)
    {
        if(!canMove) return;
        Debug.Log(value);
    }

    private void Jump()
    {
        if(!canMove) return;
        Debug.Log("Jump");
    }

    private void Attack()
    {
        if(!canMove) return;
        Debug.Log("Attack");
    }

    private void Interact()
    {
        Debug.Log("Interact");
    }

    private void DreamShift()
    {
        if(!canDreamShift) return;
        if(!canMove) return;
        Debug.Log("DreamShift");
    }

    private void Glide(bool pressed)
    {
        if(!hasGlide) return;
        if(!canMove) return;
        Debug.Log("Glide");
    }
}
