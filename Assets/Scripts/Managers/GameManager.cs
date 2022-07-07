using Game.Level;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private PlayerController _player;
        [SerializeField] private GravityManager _gravityManager;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private LevelManager _levelManager;

        #endregion

        #region Unity Methods

        private void Start()
        {
            Initialize();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            SubsribeEvents();
            PoolManager.Instance.Initialize();
            _mapGenerator.SetupCircle();

            _levelManager.Initialize(_mapGenerator.GetCirclePoints(), _gravityManager);

            _player.Initilize(_mapGenerator.GetWalls(), _gravityManager);
            _gravityManager.SubscribeToGravity(_player.Rigidbody2D);

            StartGame();
        }

        private void SubsribeEvents()
        {
            EventManager.Instance.OnCollideEnemy.Register(RestartLevel);
            EventManager.Instance.OnLevelCompleted.Register(NextLevel);
            EventManager.Instance.OnFirstInputDetected.Register(ActivateLevel);
            EventManager.Instance.OnProgressIncreased.Register(_mapGenerator.UpdateProgress);
        }

        private void StartGame()
        {
            _levelManager.LoadLevel();
            _gravityManager.SetActiveGravity(true);
        }

        private void Reset()
        {
            _player.SetActiveController(false);
            _levelManager.ResetLevel();
            _gravityManager.SetActiveGravity(false);
            PoolManager.Instance.ResetAll();
            _mapGenerator.Reset();
        }

        private void ActivateLevel()
        {
            _player.SetActiveController(true);
            _levelManager.EnableLevel();
        }

        private void NextLevel()
        {
            Reset();
            _levelManager.IncrementLevel();
            StartGame();
        }

        private void RestartLevel()
        {
            Reset();
            StartGame();
        }

        #endregion
    }
}
