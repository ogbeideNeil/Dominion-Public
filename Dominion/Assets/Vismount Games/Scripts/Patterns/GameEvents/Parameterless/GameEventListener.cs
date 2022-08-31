using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField]
    private string description;

    [SerializeField]
    [Tooltip("Specify the game event (scriptable object) which will raise the event")]
    private GameEvent gameEvent;

    [SerializeField]
    private UnityEvent eventResponse;
#pragma warning restore CS0649

    public void SetParameters(GameEvent gameEvent, UnityEvent response)
    {
        SetGameEvent(gameEvent);
        SetResponse(response);
    }

    public void SetGameEvent(GameEvent newGameEvent)
    {
        gameEvent?.UnRegisterListener(this);
        gameEvent = newGameEvent;
        gameEvent?.RegisterListener(this);
    }

    public void SetResponse(UnityEvent newResponse)
    {
        eventResponse = newResponse;
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

    public virtual void OnEventRaised()
    {
        eventResponse?.Invoke();
    }
}