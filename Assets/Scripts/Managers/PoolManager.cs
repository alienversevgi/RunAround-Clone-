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
        [SerializeField] private GameObject _shooterPrefab;
        [SerializeField] private GameObject _bulletPrefab;

        public Pool<Square> SquarePool { get; private set; }
        public Pool<Triangle> TrianglePool { get; private set; }
        public Pool<Shooter> ShooterPool { get; private set; }
        public Pool<Bullet> BulletPool { get; private set; }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            SquarePool = new Pool<Square>(new PrefabFactory<Square>(_squarePrefab, "Squares"), 3);
            TrianglePool = new Pool<Triangle>(new PrefabFactory<Triangle>(_trianglePrefab, "Triangles"), 3);
            ShooterPool = new Pool<Shooter>(new PrefabFactory<Shooter>(_shooterPrefab, "Shooter"), 2);
            BulletPool = new Pool<Bullet>(new PrefabFactory<Bullet>(_bulletPrefab, "Bullet"), 15);
        }

        public void ResetAll()
        {
            SquarePool.ReleaseAll();
            TrianglePool.ReleaseAll();
            ShooterPool.ReleaseAll();
            BulletPool.ReleaseAll();
        }

        #endregion
    }
}