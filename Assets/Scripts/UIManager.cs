using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text PercentageText;

    public void SetPercentageText(sbyte value)
    {
        PercentageText.text = string.Concat("%", value.ToString());
    }
}
