using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TParam">Custom data-type in the Base Game Event</typeparam>
/// <typeparam name="TEvent">Base Game Event</typeparam>
/// <typeparam name="TResponse">The unity event</typeparam>
public abstract class BaseGameEventListener<TParam, TEvent, TResponse> : MonoBehaviour,
    IGameEventListener<TParam> where TEvent : BaseGameEvent<TParam> where TResponse : UnityEvent<TParam>
{
    [SerializeField]
    private TEvent gameEvent;

    [SerializeField]
    private TResponse response;

    public void SetParameters(TEvent gameEvent, TResponse response)
    {
        SetGameEvent(gameEvent);
        SetResponse(response);
    }

    public void SetGameEvent(TEvent newGameEvent)
    {
        gameEvent?.UnRegisterListener(this);
        gameEvent = newGameEvent;
        gameEvent?.RegisterListener(this);
    }

    public void SetResponse(TResponse newResponse)
    {
        response = newResponse;
    }

    private void OnEnable()
    {
        gameEvent?.RegisterListener(this);
    }

    private void OnDestroy()
    {
        gameEvent?.UnRegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent?.UnRegisterListener(this);
    }

    public virtual void OnEventRaised(TParam parameters)
    {
        response?.Invoke(parameters);
    }
}
