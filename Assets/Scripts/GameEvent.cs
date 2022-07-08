using System;

public struct GameEvent 
{
    public event Action EventListeners;

    public void Raise()
    {
        EventListeners.Invoke();
    }

    public void Register(Action listener)
    {
        EventListeners += listener;
    }

    public void UnregisterListener(Action listener)
    {
        EventListeners -= listener;
    }
}
public struct GameEvent<T>
{
    public event Action<T> EventListeners;

    public void Raise(T value)
    {
        EventListeners.Invoke(value);
    }

    public void Register(Action<T> listener)
    {
        EventListeners += listener;
    }

    public void UnregisterListener(Action<T> listener)
    {
        EventListeners -= listener;
    }
}