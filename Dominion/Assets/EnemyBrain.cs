using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private Damageable damageable;
    private Stunnable stunnable;
    private Animator animator;
    private Vector3 lookAt;
    private Quaternion rotation;
    private RaycastHit hit;

    void Awake()
    {
        damageable = GetComponent<Damageable>();
        stunnable = GetComponent<Stunnable>();
        animator = GetComponent<Animator>();
    }

    public void Swipe()
    {
        Debug.Log("Swipe");
        Gizmos.color = Color.yellow;
        Physics.SphereCast(gameObject.transform.position, 5, transform.forward, out hit, 10);
        Debug.Log(hit);
        Gizmos.DrawSphere(gameObject.transform.position, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Katana")
        {
            damageable.Damage(10, other.gameObject);
            //https://answers.unity.com/questions/161053/making-an-object-rotate-to-face-another-object.html
            lookAt = other.GetComponent<Katana>().parent.transform.position - gameObject.transform.position;
            lookAt.y = 0;
            rotation = Quaternion.LookRotation(lookAt);
            transform.rotation = rotation;
            damageable.Damage(10, other.gameObject);
            stunnable.Stun(1f);
        }
    }
}
