using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knockbackable : MonoBehaviour, IKnockbackable
{

    private bool stunned;
    private NavMeshAgent agent;
    private Animator animator;
    public void Knockback(Vector3 direction, float strength)
    {

    }
    public void Knockback()
    {
        StartCoroutine(ApplyKnockback());
    }

    // Start is called before the first frame update
    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public bool GetState()
    {
        return stunned;
    }

    IEnumerator ApplyKnockback()
    {
        stunned = true;
        agent.speed = 10;
        agent.angularSpeed = 0;
        agent.acceleration = 20;
        animator.enabled = false;
        yield return new WaitForSeconds(1f);
        stunned = false;
        agent.speed = 3.5f;
        agent.angularSpeed = 120;
        agent.acceleration = 8; 
        animator.enabled = true;
    }
}
