using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CountDown : MonoBehaviour
    {
        #region Fields
        
        private const int SECONDS =3;

        [SerializeField] private Text _countDownText;
        private Coroutine _countDownCoroutine;

        #endregion

        #region Public Methods

        public void Play()
        {
            if (_countDownCoroutine != null)
            {
                StopCoroutine(_countDownCoroutine);
            }

            _countDownCoroutine = StartCoroutine(CountDownCoroutine());
        }

        #endregion

        #region Private Methods

        private IEnumerator CountDownCoroutine()
        {
            for (int i = SECONDS; i > 0; i--)
            {
                _countDownText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1.0f);
            }

            _countDownText.text = "GO";
            yield return new WaitForSecondsRealtime(1.0f);
            _countDownText.text = string.Empty;
            _countDownCoroutine = null;
            EventManager.Instance.OnCountDownFinished.Raise();
        }

        #endregion
    }
}
