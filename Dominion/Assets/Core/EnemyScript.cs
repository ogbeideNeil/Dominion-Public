using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IKnockbackable
{
    private Rigidbody rb;
    public void Knockback(Vector3 direction, float strength)
    {
        rb.AddForce(direction * strength);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
