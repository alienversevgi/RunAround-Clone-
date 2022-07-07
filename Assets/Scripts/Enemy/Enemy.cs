using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnverPool;
using Random = UnityEngine.Random;

namespace Game.Level
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(LookAt2D))]
    public abstract class Enemy : Entity
    {
        #region Fields

        private const sbyte COLOR_SWITCH_COUNT = 3;
        private const float COLOR_SWITCH_SECONDS = 0.3f;

        public Rigidbody2D _rigidbody { get; protected set; }

        protected Collider2D _collider;
        protected SpriteRenderer _renderer;
        protected List<Vector2> _circlePoints;
        protected bool _isColorSwitchAnimationFinished;

        #endregion

        #region Unity Methods

        public virtual void Awake()
        {
            _collider = this.GetComponent<Collider2D>();
            _renderer = this.GetComponent<SpriteRenderer>();
            _rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player") && collision.otherCollider.CompareTag("Enemy"))
            {
                DisableAction();
                EventManager.Instance.OnCollideEnemy.Raise();
            }
        }

        #endregion

        #region Methods

        public abstract void EnableAction();
        public abstract void DisableAction();

        public void SetCirclePoints(List<Vector2> circlePoints)
        {
            this._circlePoints = circlePoints;
        }

        protected void ShowColorSwitchAnimation()
        {
            _isColorSwitchAnimationFinished = false;

            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            StartCoroutine(ColorSwitch());
        }

        protected Vector2 GetRandomPosition()
        {
            return _circlePoints[Random.Range(0, _circlePoints.Count)];
        }

        public virtual IEnumerator ColorSwitch()
        {
            _renderer.color = Color.yellow;
            for (sbyte i = 0; i < COLOR_SWITCH_COUNT; i++)
            {
                _renderer.enabled = false;
                yield return new WaitForSecondsRealtime(COLOR_SWITCH_SECONDS);
                _renderer.enabled = true;
                yield return new WaitForSecondsRealtime(COLOR_SWITCH_SECONDS);
            }

            _renderer.color = Color.black;
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
            _isColorSwitchAnimationFinished = true;
        }

        #endregion
    }
}