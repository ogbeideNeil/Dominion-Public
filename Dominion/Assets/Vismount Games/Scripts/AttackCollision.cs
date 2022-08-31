using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AttackCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject damageSource;

    [SerializeField]
    private List<Layers> attackables;

    [SerializeField]
    private float damageAmount;

    [SerializeField]
    private bool enabledOnAwake = false;

    [SerializeField]
    private UnityEvent onEnabledEvents;

    private HashSet<GameObject> hitObjects;

    private void Awake()
    {
        hitObjects = new HashSet<GameObject>();
        enabled = enabledOnAwake;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        foreach (Layers layer in attackables.Where(layer => (int) layer == other.gameObject.layer && !hitObjects.Contains(other.gameObject)))
        {
            hitObjects.Add(other.gameObject);
            other.gameObject.GetComponent<Damageable>()?.Damage(damageAmount, damageSource);
        }
    }

    private void OnEnable()
    {
        hitObjects.Clear();
        onEnabledEvents.Invoke();
    }

    private void OnDisable()
    {
        hitObjects.Clear();
    }
}