using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    [SerializeField]
    [Header("Descriptive Information (optional)")]
    [ContextMenuItem("Reset Name", "ResetName")]
    private string eventName;

    private List<GameEventListener> listeners = new List<GameEventListener>();

    [ContextMenu("Raise Event")]
    public virtual void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnRegisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}