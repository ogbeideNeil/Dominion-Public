public interface IGameEventListener<TParam>
{
    public void OnEventRaised(TParam parameters);
}