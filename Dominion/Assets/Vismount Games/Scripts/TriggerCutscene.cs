using UnityEngine;

public class TriggerCutscene : MonoBehaviour, ITrigger
{
    [SerializeField]
    [Tooltip("Add box collider to make this a collideable trigger")]
    private DirectorEvent triggerEvent;

    [SerializeField]
    private DirectorData cutsceneData;

    [SerializeField]
    [Tooltip("Delete this behaviour after it triggers")]
    private bool triggerOnce = true;

    public DirectorData CutsceneData => cutsceneData;

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        Trigger();
    }

    public void Trigger()
    {
        triggerEvent.Raise(cutsceneData);

        if (triggerOnce)
        {
            Destroy(this);
        }
    }
}
