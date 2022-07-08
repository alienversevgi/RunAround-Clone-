
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public GameEvent OnCollideEnemy;
    public GameEvent OnLevelCompleted;
    public GameEvent OnFirstInputDetected;
    public GameEvent OnProgressIncreased;
    public GameEvent OnCountDownFinished;

    public GameEvent<Rigidbody2D> OnSubscribeGravity;
    public GameEvent<Rigidbody2D> OnUnsubscribeGravity;
}