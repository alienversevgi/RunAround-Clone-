using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private Text levelText;

    public int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt("level", 0);
        }
        private set
        {

            PlayerPrefs.SetInt("level", value);
        }
    }

    private List<LevelData> _levels;
    private LevelData _currentLevelData => _levels[CurrentLevel];

    private List<Vector2> _circlePoints;
    private GravityManager _gravityManager;
    private List<Enemy> _allEnemys;

    #endregion

    #region Public Methods

    public void Initialize(List<Vector2> circlePoints, GravityManager gravityManager)
    {
        _levels = Resources.LoadAll<LevelData>("Levels").ToList();
        _allEnemys = new List<Enemy>();
        _circlePoints = circlePoints;
        _gravityManager = gravityManager;
    }

    public void LoadLevel()
    {
        levelText.text = $"Level-{CurrentLevel}";
  
        foreach (SquareData squareData in _currentLevelData.Squares)
        {
            Square square = PoolManager.Instance.SquarePool.Allocate();
            square.Initialize(squareData);
            square.SetPositionAndEnable(squareData.Position);

            _allEnemys.Add(square);
        }

        foreach (TriangleData triangleData in _currentLevelData.Triangles)
        {
            Triangle triangle = PoolManager.Instance.TrianglePool.Allocate();
            triangle.Initialize(triangleData.TeleportRate);
            triangle.SetPositionAndEnable(triangleData.Position);

            _allEnemys.Add(triangle);
        }

        foreach (Enemy enemy in _allEnemys)
        {
            enemy.SetCirclePoints(_circlePoints);
            enemy.GetComponent<LookAt2D>().SetTarget(_gravityManager.transform);
            _gravityManager.SubscribeToGravity(enemy.rigidbody);
         
        }
    }

    public void EnableLevel()
    {
        _allEnemys.ForEach(enemy => enemy.EnableAction());  
    }

    public void IncrementLevel()
    {
        CurrentLevel++;
        if (CurrentLevel > _levels.Count - 1)
        {
            CurrentLevel = Random.Range(0, _levels.Count);
        }
    }

    public void ResetLevel()
    {
        foreach (Enemy enemy in _allEnemys)
        {
            _gravityManager.UnsubscribeToGravity(enemy.rigidbody);
            enemy.DisableAction();
        }

        _allEnemys.Clear();
    }

    #endregion
}