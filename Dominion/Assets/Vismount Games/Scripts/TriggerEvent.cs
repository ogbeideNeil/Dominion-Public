using UnityEngine;

public class TriggerEvent : MonoBehaviour, ITrigger
{
    [SerializeField]
    [Tooltip("Add box collider to make this a collideable trigger")]
    private GameEvent triggerEvent;

    [SerializeField]
    [Tooltip("Delete this behaviour after it triggers")]
    private bool triggerOnce = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        Trigger();
    }

    public void Trigger()
    {
        triggerEvent.Raise();
        if (triggerOnce)
        {
            Destroy(this);
        }
    }
}
