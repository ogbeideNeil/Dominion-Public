using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class CombatHandler : MonoBehaviour
{
    public static CombatHandler instance;
    public Animator animator;
    public Katana katana;

    public bool canRecieveInput;
    public bool inputRecieved;
    public bool isAttacking;
    public bool isDodging;
    public bool isDowned;
    public bool isJumping;
    public bool isDoubleJumping;
    public bool isSkilling;

    private PlayerActions playerActions;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        playerActions = new PlayerActions();

        playerActions.PlayerCombat.Attack.performed += PerformedAttack;
        playerActions.PlayerCombat.Dodge.performed += PerformedDodge;
        playerActions.PlayerCombat.Jump.performed += PerformedJump;
        playerActions.PlayerCombat.Skill.performed += PerformedSkill;
    }

    //private void Update()
    //{
    //    //Katana katana = GetComponentInChildren<Katana>();
    //    //katana.setAttacking(isSkilling);
    //}

    public void Attacked(float delay)
    {
        //katana.EnableCollider(delay);
    }

    public void ControlsEnabled(bool enable)
    {
        if (enable)
        {
            playerActions.PlayerCombat.Enable();
        }
        else
        {
            playerActions.PlayerCombat.Disable();
        }
    }

    private void PerformedAttack(InputAction.CallbackContext context)
    {
        isAttacking = true;
    }

    private void PerformedDodge(InputAction.CallbackContext context)
    {
        isDodging = true;
    }

    private void PerformedJump(InputAction.CallbackContext context)
    {
        if (isJumping)
        {
            isDoubleJumping = true;
        }
        else
        {
            isJumping = true;
        }
    }

    private void PerformedSkill(InputAction.CallbackContext context)
    {
        isSkilling = true;
    }
}
