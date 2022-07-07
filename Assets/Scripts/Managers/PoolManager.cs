using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnverPool;
using Game.Level;

namespace Game
{
    public class PoolManager : Singleton<PoolManager>
    {
        #region Fields

        [SerializeField] private GameObject _squarePrefab;
        [SerializeField] private GameObject _trianglePrefab;

        public Pool<Square> SquarePool { get; private set; }
        public Pool<Triangle> TrianglePool { get; private set; }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            SquarePool = new Pool<Square>(new PrefabFactory<Square>(_squarePrefab, "Squares"), 5);
            TrianglePool = new Pool<Triangle>(new PrefabFactory<Triangle>(_trianglePrefab, "Triangles"), 5);
        }

        public void ResetAll()
        {
            SquarePool.ReleaseAll();
            TrianglePool.ReleaseAll();
        }

        #endregion
    }
}