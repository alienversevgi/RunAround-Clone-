using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PercentageIndicator : MonoBehaviour
    {
        [SerializeField] private Text percentageText;

        public void SetPercentageText(sbyte percentage)
        {
            percentageText.text = string.Concat("%", percentage.ToString());
        }
    }
}