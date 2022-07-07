using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Enemy
{
    // = 5.0f
    private float _teleportRate;

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
}