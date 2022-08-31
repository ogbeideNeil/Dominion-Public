using System;
using UnityEngine;

namespace GD.ScriptableTypes
{
    /// <summary>
    /// Stores event time variables e.g. To play an event at a certain time
    /// </summary>
    [CreateAssetMenu(fileName = "ListEventTimeVariable", menuName = "Scriptable Objects/Collections/List/Event Time")]
    public class ListEventTimeVariable : ListVariable<EventTime>
    {
    }

    [Serializable]
    public class EventTime
    {
        [SerializeField]
        private float time;

        [SerializeField]
        private GameEvent gameEvent;

        public float Time => time;
        public GameEvent GameEvent => gameEvent;
    }

}