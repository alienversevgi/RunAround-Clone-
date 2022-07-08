using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public class Square : Enemy
    {
        #region Fields

        private float _speed;
        private bool _isMoveable;
        private DirectionType _direction;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (!_isMoveable)
                return;

            if (_isEnable && _isColorSwitchAnimationFinished)
            {
                int direction = _direction == DirectionType.Right ? 1 : -1;
                this.transform.position += (transform.up * direction) * _speed * Time.deltaTime;
            }
        }

        public override void Reset()
        {
            base.Reset();
            EventManager.Instance.OnUnsubscribeGravity.Raise(_rigidbody);
        }

        #endregion

        #region Public Methods

        public void Initialize(SquareData squareData)
        {
            _speed = squareData.Speed;
            _isMoveable = squareData.IsMoveable;
            _direction = squareData.Direction;
            EventManager.Instance.OnSubscribeGravity.Raise(_rigidbody);
        }

        public override void EnableAction()
        {
            base.EnableAction();
            ShowColorSwitchAnimation();
        }

        #endregion
    }
}