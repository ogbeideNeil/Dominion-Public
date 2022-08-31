using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CombatHandler.instance.isAttacking)
        {
            CombatHandler.instance.animator.Play("1");
        } else if (CombatHandler.instance.isDodging)
        {
            CombatHandler.instance.animator.Play("BackDodge");
        } else if (CombatHandler.instance.isJumping)
        {
            CombatHandler.instance.animator.Play("Jump");
        } else if (CombatHandler.instance.isSkilling)
        {
            CombatHandler.instance.animator.Play("Skill");
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CombatHandler.instance.isAttacking = false;
        CombatHandler.instance.isDodging = false;
        CombatHandler.instance.isJumping = false;
        CombatHandler.instance.isSkilling = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
