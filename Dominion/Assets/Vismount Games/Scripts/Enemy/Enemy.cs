using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BehaviourTreeRunner), typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, ICollider, IDeathResponse, IDamageResponse
{
#pragma warning disable CS0649
    [SerializeField]
    protected ZoneObjectData zoneData;

    [SerializeField]
    protected float visionRange = 20f;

    [SerializeField]
    protected List<EnemyAttack> attacks;

    [SerializeField]
    protected Sound damageSound;

    [Header("Wander Settings")]
    [SerializeField]
    protected float wanderRadius;

    [SerializeField]
    protected float minMoveDistance;

    [SerializeField]
    protected int maxAttempts = 4;

    [SerializeField]
    protected LayerMask groundLayer;
#pragma warning restore CS0649

    protected const string AnimatorVelocity = "Velocity";
    protected const string AnimatorWalkingBackwards = "WalkingBackwards";

    protected NavMeshAgent agent;
    protected CapsuleCollider mainCollider;
    protected Animator animator;
    protected EnemyAttack chosenAttack;
    protected Damageable damageable;
    protected AnimatedBar healthBar;
    protected float nextAttackTime;


    public bool AttackFinished { get; protected set; }
    public bool CanAttack { get; protected set; }
    public Vector3 CirclingPosition { get; protected set; }
    public float CurrentAttackDistance => GetClosestPossibleAttack().AttackRange;
    public bool EngagePlayer { get; set; }
    public Vector3 LastKnownPlayerPosition { get; protected set; }
    public Collider GroundCollider { get; protected set; }
    public bool PlayerCanSeeMe { get; protected set; }
    public bool PlayerPositionKnown { get; protected set; }
    public Vector3 StartPosition { get; protected set; }

    public LayerMask GroundLayer => groundLayer;
    public int MaxAttempts => maxAttempts;
    public float MinMoveDistance => minMoveDistance;
    public virtual EnemyType EnemyType => EnemyType.Default;
    public float VisionRange => visionRange;
    public float WanderRadius => wanderRadius;
    public ZoneObjectData ZoneData => zoneData;

    public virtual bool Attack()
    {
        if (!CanAttack) return false;

        chosenAttack = GetClosestPossibleAttack();

        nextAttackTime = Time.time + chosenAttack.AttackCooldown;
        AttackFinished = false;
        CanAttack = false;
        transform.rotation *= Quaternion.AngleAxis(chosenAttack.YRotationOffset, Vector3.up);

        StartCoroutine(EnableAttackCollision(chosenAttack.EnableCollisionAfterSeconds, chosenAttack.AttackCollision));

        animator.SetTrigger(chosenAttack.AnimationTrigger);

        return true;
    }

    private IEnumerator EnableAttackCollision(float seconds, AttackCollision attackCollision)
    {
        yield return new WaitForSeconds(seconds);
        attackCollision.enabled = true;

    }

    public Collider GetCollider()
    {
        return mainCollider;
    }

    public void OnDeath()
    {
        GetComponent<BehaviourTreeRunner>().enabled = false;
        mainCollider.enabled = false;
        healthBar.gameObject.SetActive(false);
        EnemyDirector.UnRegisterEnemy(this);

        StartCoroutine(SinkAndDestroy());

        agent.enabled = false;
        //visionSphere.enabled = false;
        enabled = false;
    }

    public void OnDamage(GameObject damageSource) 
    {
        if (!PlayerPositionKnown)
        {
            EnemyDirector.AlertZone(zoneData);
            PlayerPositionKnown = true;
        }

        SoundManager.PlaySound(damageSound, transform.position);

        float newFillPercent = damageable.Health / damageable.MaxHealth;

        if (newFillPercent < 0)
        {
            newFillPercent = 0;
        }

        healthBar.SetFillAmount(newFillPercent);
    }

    public bool IsDead()
    {
        return !enabled;
    }

    public virtual void FinishAttack()
    {
        AttackFinished = true;
        chosenAttack.AttackCollision.enabled = false;
    }

    public void TrackPlayerPosition(Vector3 position)
    {
        LastKnownPlayerPosition = position;
        PlayerPositionKnown = true;
    }

    public EnemyAttack GetClosestPossibleAttack()
    {
        float distance = Vector3.Distance(transform.position, LastKnownPlayerPosition);

        foreach (EnemyAttack attack in attacks)
        {
            if (distance <= attack.AttackRange)
            {
                return attack;
            }
        }

        return attacks.Last();
    }

    protected void Awake()
    {
        GroundCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
        agent = this.GetComponentAssertNotNull<NavMeshAgent>();
        animator = this.GetComponentWarnNull<Animator>();
        mainCollider = this.GetComponentAssertNotNull<CapsuleCollider>();
        damageable = this.GetComponentAssertNotNull<Damageable>();
        healthBar = this.GetComponentInChildrenAssertNotNull<AnimatedBar>();
        StartPosition = transform.position;
        CanAttack = true;
        AttackFinished = true;

        healthBar.gameObject.SetActive(false);
        attacks = attacks.OrderBy(attack => attack.AttackRange).ToList();
    }

    protected void Start()
    {
        EnemyDirector.RegisterEnemy(this);
    }

    protected void Update()
    {
        CheckPlayer();

        if (!agent.updateRotation)
        {
            RotateTowardsPlayer();
        }

        if (!CanAttack && AttackFinished)
        {
            if (Time.time >= nextAttackTime)
            {
                CanAttack = true;
            }
        }

        float velocity = agent.velocity.magnitude / agent.speed;

        if (velocity > 0)
        {
            Vector3 nextPosition = transform.position + agent.velocity - transform.position;
            bool isWalkingBackwards = Vector3.Dot(transform.forward.normalized, nextPosition.normalized) < 0;

            animator.SetBool(AnimatorWalkingBackwards, isWalkingBackwards);
        }
        animator.SetFloat(AnimatorVelocity, velocity);
    }

    private void RotateTowardsPlayer()
    {
        if (!AttackFinished) return;

        Vector3 newDirection = Vector3.RotateTowards(
            transform.forward,
            (LastKnownPlayerPosition - transform.position).normalized,
            agent.angularSpeed/60 * Time.deltaTime,
            0f);

        Quaternion newRotation = Quaternion.LookRotation(newDirection);
        newRotation.x = transform.rotation.x;
        newRotation.z = transform.rotation.z;

        transform.rotation = newRotation;
    }

    protected virtual void CheckPlayer()
    {
        if (Vector3.Distance(transform.position, EnemyDirector.Player.position) <= visionRange)
        {
            if (!PlayerPositionKnown)
            {
                EnemyDirector.AlertZone(zoneData);
                PlayerPositionKnown = true;
            }

            LastKnownPlayerPosition = EnemyDirector.Player.position;
            healthBar.gameObject.transform.LookAt(EnemyDirector.PlayerCamera);
            healthBar.gameObject.transform.Rotate(Vector3.up, 180);
            healthBar.gameObject.SetActive(true);
        }
        else
        {
            EnemyDirector.TrackPosition(EnemyDirector.Player.position);
            healthBar.gameObject.SetActive(false);
        }
    }

    private IEnumerator SinkAndDestroy()
    {
        int times = 30;
        int counter = 0;

        yield return new WaitForSeconds(2f);

        while (counter < times)
        {
            Vector3 position = transform.position;
            position.y -= 0.05f;
            transform.position = position;

            counter += 1;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}

[Serializable]
public class EnemyAttack
{
    [SerializeField]
    private AttackCollision attackCollision;

    [SerializeField]
    private string animationTrigger;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float enableCollisionAfterSeconds;

    [SerializeField]
    private float yRotationOffset;

    public AttackCollision AttackCollision => attackCollision;
    public string AnimationTrigger => animationTrigger;
    public float AttackCooldown => attackCooldown;
    public float AttackRange => attackRange;

    public float EnableCollisionAfterSeconds => enableCollisionAfterSeconds;
    public float YRotationOffset => yRotationOffset;
}
