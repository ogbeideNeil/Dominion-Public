using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom multi-parameter event passing
/// 
/// Usage:
/// Create a class that inherits this using a custom data-type. (Remember to include this in the context menu)
/// Also create a class that inherits UnityEvent of the same data-type. (Serializable Class attribute)
/// Create a class that inherits the BaseGameEventListener
/// </summary>
/// 
/// <typeparam name="TParam">Custom data-type</typeparam>
/// <see cref="https://www.youtube.com/watch?v=iXNwWpG7EhM (Author of the events system)"/>
/// <seealso cref="BaseGameEventListener{TParam,TEvent,TResponse}"/>
/// <seealso cref="IGameEventListener{TParam}"/>
public abstract class BaseGameEvent<TParam> : ScriptableObject
{
    #region Fields

    [SerializeField]
    [Header("Descriptive Information (optional)")]
    [ContextMenuItem("Reset Name", "ResetName")]
    protected string eventName;

    protected readonly List<IGameEventListener<TParam>> listeners = new List<IGameEventListener<TParam>>();

    #endregion Fields

    [ContextMenu("Raise Event")]
    public virtual void Raise(TParam parameters)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(parameters);
        }
    }

    public void RegisterListener(IGameEventListener<TParam> listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnRegisterListener(IGameEventListener<TParam> listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
