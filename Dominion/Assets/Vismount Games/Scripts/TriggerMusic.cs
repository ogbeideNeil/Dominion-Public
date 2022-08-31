using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusic : MonoBehaviour, ITrigger
{
    [SerializeField]
    [Tooltip("Add box collider to make this a collideable trigger")]
    private Sound triggerMusic;

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
        MusicManager.Play(triggerMusic);

        if (triggerOnce)
        {
            Destroy(this);
        }
    }
}
