using System;
using UnityEngine;

namespace GD.ScriptableTypes
{
    /// <summary>
    /// Stores sound time variables e.g. To play a sound at a certain time
    /// </summary>
    [CreateAssetMenu(fileName = "ListSoundTimeVariable", menuName = "Scriptable Objects/Collections/List/Sound Time")]
    public class ListSoundTimeVariable : ListVariable<SoundTime>
    {
    }

    [Serializable]
    public class SoundTime
    {
        [SerializeField]
        private float time;

        [SerializeField]
        private Sound sound;

        public float Time => time;
        public Sound Sound => sound;
    }

}