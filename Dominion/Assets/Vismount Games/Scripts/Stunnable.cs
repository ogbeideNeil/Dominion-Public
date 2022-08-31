using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Stunnable : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool crRunning = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Stun(float stunTime)
    {
        navMeshAgent.isStopped = true;

        if (crRunning)
        {
            StopCoroutine(StunEnemy(stunTime));
        }
        StartCoroutine(StunEnemy(stunTime));

        crRunning = false;


    }

    IEnumerator StunEnemy(float stunTime)
    {
        crRunning = true;
        animator.SetTrigger("TriggerStun");
        yield return new WaitForSeconds(stunTime);
        navMeshAgent.isStopped = false;
    }
}
