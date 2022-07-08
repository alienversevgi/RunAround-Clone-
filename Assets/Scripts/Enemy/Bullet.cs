using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EnverPool;

namespace Game.Level
{
    public class Bullet : Entity
    {
        #region Fields

        private Rigidbody2D rigidbody;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerController playerController = collision.collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                EventManager.Instance.OnCollideEnemy.Raise();
                Reset();
            }
            else
            {
                Utility.Timer.Instance.StartTimer(3.0f, () => PoolManager.Instance.BulletPool.Release(this));
            }
        }

        #endregion

        #region Public Methods

        public void Force(float value)
        {
            rigidbody.AddForce(this.transform.up * value * 100 * Time.deltaTime);
        }

        #endregion
    }
}
