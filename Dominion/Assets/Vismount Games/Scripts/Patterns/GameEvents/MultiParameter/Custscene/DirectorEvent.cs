using System;
using GD.ScriptableTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "DirectorEvent", menuName = "Scriptable Objects/Events/Director")]
public class DirectorEvent : BaseGameEvent<DirectorData>
{
}

[Serializable]
public class UnityDirectorEvent : UnityEvent<DirectorData>
{
}

[Serializable]
public struct DirectorData
{
    [SerializeField]
    private PlayableDirector director;

    [SerializeField]
    private ListSoundTimeVariable soundTimes;

    [SerializeField]
    private ListEventTimeVariable eventTimes;

    public PlayableDirector Director => director;
    public ListSoundTimeVariable SoundTimes => soundTimes;
    public ListEventTimeVariable EventTimes => eventTimes;

    public override string ToString()
    {
        return $"{Director.name}";
    }
}