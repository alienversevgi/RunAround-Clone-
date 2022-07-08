using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public class Triangle : Enemy
    {
        #region Fields

        private float _teleportRate;

        #endregion

        #region Unity Methods

        public override void Reset()
        {
            base.Reset();
            EventManager.Instance.OnUnsubscribeGravity.Raise(_rigidbody);
      
        }

        #endregion

        #region Public Methods

        public void Initialize(float teleportRate)
        {
            _teleportRate = teleportRate;
            EventManager.Instance.OnSubscribeGravity.Raise(_rigidbody);
        }

        public override void EnableAction()
        {
            base.EnableAction();
            StartCoroutine(TransportCoroutine());
        }

        public override void DisableAction()
        {
            base.DisableAction();
            StopAllCoroutines();
        }

        #endregion

        #region Private Methods

        private IEnumerator TransportCoroutine()
        {
            ShowColorSwitchAnimation();
            while (true)
            {
                yield return new WaitForSecondsRealtime(_teleportRate);
                SetPositionAndEnable(GetRandomPosition());
                ShowColorSwitchAnimation();
            }
        }

        #endregion
    }
}