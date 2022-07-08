using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public class Shooter : Enemy
    {
        #region Fields

        [SerializeField] private Transform _bulletSpawnPoint;
        private float _angle;
        private ShooterData _shooterData;
        private Coroutine _shootCoroutine;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (!_isEnable)
                return;

            Move();
        }

        #endregion

        #region Public Methods

        public void Initialize(ShooterData shooterData)
        {
            _shooterData = shooterData;
            _angle = shooterData.Angle;
        }

        public override void DisableAction()
        {
            base.DisableAction();
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }

        public override void EnableAction()
        {
            base.EnableAction();
            _shootCoroutine = StartCoroutine(ShootCoroutine());
        }

        #endregion

        #region Private Methods

        private void Move()
        {
            _angle += _shooterData.Speed * Time.deltaTime;
            Vector2 offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * _shooterData.Radiant;
            this.transform.position = _shooterData.Position + offset;
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(_shooterData.FireRate);
                Bullet bullet = PoolManager.Instance.BulletPool.Allocate();

                bullet.transform.eulerAngles = this.transform.eulerAngles;
                bullet.SetPositionAndEnable(_bulletSpawnPoint.position);
          
                bullet.Force(_shooterData.BulletForce);
            }
        }

        private float AngleTo(Vector2 pos, Vector2 target)
        {
            Vector2 diference = Vector2.zero;

            if (target.x > pos.x)
                diference = target - pos;
            else
                diference = pos - target;

            return Vector2.Angle(Vector2.right, diference);
        }

        #endregion
    }
}
