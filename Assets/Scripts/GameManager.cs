using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GravityManager gravityManager;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private List<Enemy> enemys;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        EventManager.Instance.OnCollideEnemy.Register(RestartLevel);
        EventManager.Instance.OnLevelCompleted.Register(NextLevel);
        EventManager.Instance.OnFirstInputDetected.Register(ActivateLevel);

        PoolManager.Instance.Initialize();
        mapGenerator.SetupCircle();

        levelManager.Initialize(mapGenerator.GetCirclePoints(), gravityManager);

        player.Initilize(mapGenerator.GetWalls(), gravityManager);
        player.ProgressIncreased += mapGenerator.UpdateProgress;
        gravityManager.SubscribeToGravity(player.Rigidbody2D);

        StartGame();
    }

    private void StartGame()
    {
        levelManager.LoadLevel();
        gravityManager.SetActiveGravity(true);
    }

    private void GameOver()
    {
        player.SetActiveController(false);
        levelManager.ResetLevel();
        gravityManager.SetActiveGravity(false);
        PoolManager.Instance.ResetAll();
        mapGenerator.Reset();
    }

    private void ActivateLevel()
    {
        player.SetActiveController(true);
        levelManager.EnableLevel();
    }

    private void NextLevel()
    {
        GameOver();
        levelManager.IncrementLevel();
        StartGame();
    }

    private void RestartLevel()
    {
        GameOver();
        StartGame();
    }
}
