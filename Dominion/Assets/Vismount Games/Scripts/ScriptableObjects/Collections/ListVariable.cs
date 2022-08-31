using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GD.ScriptableTypes
{
    public abstract class ListVariable<T> : NumberVariable<T>
    {
        [SerializeField]
        private List<T> list;

        [SerializeField]
        [Tooltip("Don't modify this value")]
        private int previousCount;

        public IReadOnlyList<T> List => list;

        public int Count => List.Count;

        public void Add(T obj)
        {
            list.Add(obj);
        }

        public void Remove(T obj)
        {
            list.Remove(obj);
        }

        public void Clear()
        {
            list.Clear();
        }

        private void OnValidate()
        {
            if (Count > previousCount)
            {
                list[Count - 1] = default;
            }

            previousCount = Count;
        }

        //Sort(comparable), Remove(predicate)
    }
}