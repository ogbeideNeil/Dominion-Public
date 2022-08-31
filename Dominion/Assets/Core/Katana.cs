using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Katana : MonoBehaviour
{
    // Start is called before the first frame update
    bool m_Started;
    public LayerMask m_LayerMask;
    bool attacking = false;
    bool crRunning = false;
    private RaycastHit hit;
    private AttackCollision attackCollision;
    private Coroutine attackRoutine;

    [SerializeField]
    public GameObject parent;

    bool played = false;


    private void Awake()
    {
        attackCollision = GetComponent<AttackCollision>();
    }

    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
        //sound = GetComponent<SoundFXManager>();
    }

    private void Update()
    {
        if (CombatHandler.instance.isAttacking || CombatHandler.instance.isSkilling)
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }
            attackRoutine = StartCoroutine(EnableAttackCollision());
        }
        //ParticleSystem fire = GetComponentInChildren<ParticleSystem>();
        //if(attacking)
        //{
        //    fire.Play();
        //} else
        //{
        //    fire.Stop();
        //}
    }

    private IEnumerator EnableAttackCollision()
    {
        attackCollision.enabled = false;
        attackCollision.enabled = true;
        yield return new WaitForSeconds(1);
        attackCollision.enabled = false;
    }

    public void setAttacking(bool attack)
    {
        attacking = attack;
    }

    void FixedUpdate()
    {
        MyCollisions();
    }

    void MyCollisions()
    {
        ////Use the OverlapBox to detect if there are any other colliders within this box area.
        ////Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.

        //Bounds bounds = GetComponent<Collider>().bounds;
        //Collider[] hitColliders = Physics.OverlapBox(bounds.center, bounds.extents / 2, Quaternion.identity, m_LayerMask);
        //int i = 0;
        ////Check when there is a new collider coming into contact with the box
        //while (i < hitColliders.Length)
        //{
        //    if (hitColliders[i].TryGetComponent(out ICollider targetCollider) && hitColliders[i] == targetCollider.GetCollider())
        //    {
        //        //Output all of the collider names
        //        //Debug.Log("Hit : " + hitColliders[i].name + ", " + i);

        //        hitColliders[i].gameObject.GetComponent<IKnockbackable>()?.Knockback(new Vector3(1, 1, 1), 100);
        //        hitColliders[i].gameObject.GetComponent<Damageable>()?.Damage(20);
        //        hitColliders[i].gameObject.GetComponent<able>()?.Damage(20);
        //    }

        //    i++;
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(GetComponent<Collider>().enabled)
    //    { 
    //        if(Physics.SphereCast(gameObject.transform.position, 5, transform.forward, out hit, 1 << m_LayerMask))
    //        {
    //        //    //Output all of the collider names
    //        //    //Debug.Log("Hit : " + hitColliders[i].name + ", " + i);

    //        //    hitColliders[i].gameObject.GetComponent<IKnockbackable>()?.Knockback(new Vector3(1, 1, 1), 100);
    //        //    hitColliders[i].gameObject.GetComponent<Damageable>()?.Damage(20);
    //        }
    //    }

    //    //if (!CombatHandler.instance.isAttacking)
    //    //{
    //    //    played = false;
    //    //}



    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //collision.gameObject.GetComponent<Knockbackable>()?.Knockback(new Vector3(0, 1, 0), 100);
    //    Debug.Log("Hit");
    //}

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
            Gizmos.DrawWireMesh(gameObject.GetComponent<MeshFilter>().mesh, gameObject.GetComponent<BoxCollider>().bounds.size, gameObject.transform.rotation);

            Bounds bounds = GetComponent<Collider>().bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
    //public void EnableCollider(float delay)
    //{
    //    Debug.Log("Recieved");
    //    gameObject.GetComponent<BoxCollider>().enabled = true;
    //    if (crRunning) {
    //        StopCoroutine(DisableHitbox(delay));
    //    }
    //    StartCoroutine(DisableHitbox(delay));

    //    crRunning = false;
    //}


    //IEnumerator DisableHitbox(float delay)
    //{
    //    crRunning = true;
    //    yield return new WaitForSeconds(delay);
    //    gameObject.GetComponent<BoxCollider>().enabled = false;

    //}
}
