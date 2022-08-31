using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandupTransition : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        string back = "B_Rollback";
        string forward = "B_Rollup";

        float y = animator.GetFloat("yInput");
        if (CombatHandler.instance.animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("KnockbackLoop"))
        {
            back = "Rollback";
            forward = "Rollup";
        }

        if (CombatHandler.instance.isDowned)
        {
            switch (y)
            {
                case var expression when y < 0:
                    CombatHandler.instance.animator.Play(back);
                    break;
                case var expression when y > 0:
                    CombatHandler.instance.animator.Play(forward);
                    break;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CombatHandler.instance.isDowned = false;
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
