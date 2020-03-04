using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Enemy
{
    private const float POSITION_CHANGE_TIME = 5.0f;

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
        while (true)
        {
            SetPosition(GetRandomPosition());
            yield return new WaitForSecondsRealtime(POSITION_CHANGE_TIME);
        }
    }
}