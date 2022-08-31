using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;

    [SerializeField]
    private float damageCooldown = 0.2f;

    private const string AnimatorBoolIsDead = "IsDead";

    private float nextDamageableTime;

    public float Health => health;
    public float MaxHealth { get; private set; }

    private void Awake()
    {
        MaxHealth = health;
        nextDamageableTime = Time.time;
    }

    public void Damage(float amount, GameObject damageSource)
    {
        if (amount <= 0) return;
        if (Time.time < nextDamageableTime) return;

        health -= amount;
        nextDamageableTime = Time.time + damageCooldown;

        GetComponent<IDamageResponse>()?.OnDamage(damageSource);

        if (health <= 0)
        {
            GetComponent<Animator>()?.SetBool(AnimatorBoolIsDead, true);

            if (TryGetComponent(out IDeathResponse deathResponse))
            {
                if (!deathResponse.IsDead())
                {
                    deathResponse.OnDeath();
                }
            }
        }
        else
        {
            GetComponent<Stunnable>()?.Stun(1f);
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        float deltaHealth = health + amount;

        health = deltaHealth > MaxHealth
            ? MaxHealth
            : deltaHealth;
    }
}
