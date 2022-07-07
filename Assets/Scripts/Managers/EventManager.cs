
public class EventManager : Singleton<EventManager>
{
    public GameEvent OnCollideEnemy;
    public GameEvent OnLevelCompleted;
    public GameEvent OnFirstInputDetected;
}