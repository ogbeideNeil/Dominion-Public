using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSound : MonoBehaviour, ITrigger
{
    [SerializeField]
    [Tooltip("Add box collider to make this a collideable trigger")]
    private Sound triggerSound;

    [SerializeField]
    [Tooltip("Delete this behaviour after it triggers")]
    private bool triggerOnce = true;

    [SerializeField]
    [Tooltip("Play a sound at the transform of this object")]
    private bool atThisLocation = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        Trigger();
    }

    public void Trigger()
    {
        _ = atThisLocation
            ? SoundManager.PlaySound(triggerSound, transform.position)
            : SoundManager.PlaySound(triggerSound);

        if (triggerOnce)
        {
            Destroy(this);
        }
    }
}
