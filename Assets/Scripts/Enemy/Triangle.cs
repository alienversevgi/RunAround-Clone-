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

        #region Public Methods

        public void Initialize(float teleportRate)
        {
            _teleportRate = teleportRate;
        }

        public override void EnableAction()
        {
            StartCoroutine(TransportCoroutine());
        }

        public override void DisableAction()
        {
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