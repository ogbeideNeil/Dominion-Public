using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTransition : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float x = animator.GetFloat("xInput");
        float y = animator.GetFloat("yInput");
        if (CombatHandler.instance.isDodging)
        {
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                switch (x)
                {
                    case var expression when x < 0:
                        CombatHandler.instance.animator.Play("LeftDodge");
                        break;
                    case var expression when x > 0:
                        CombatHandler.instance.animator.Play("RightDodge");
                        break;
                }

            }
            else
            {
                switch (y)
                {
                    case var expression when y > 0:
                        CombatHandler.instance.animator.Play("FrontDodge");
                        break;

                    case var expression when y < 0:
                        CombatHandler.instance.animator.Play("BackDodge");
                        break;
                }
            }
        } else if (CombatHandler.instance.isAttacking)
        {
            
            CombatHandler.instance.animator.Play("1");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CombatHandler.instance.isDodging = false;
        CombatHandler.instance.isAttacking = false;
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
