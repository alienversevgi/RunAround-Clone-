using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : Enemy
{
    private const float SPEED = 3.0f;

    private bool canItMove = false;

    public override void EnableAction()
    {
        SetPosition(GetRandomPosition());
        canItMove = true;
    }

    public override void DisableAction()
    {
        canItMove = false;
    }

    private void Update()
    {
        if (isColorSwitchAnimationFinished && canItMove)
        {
            this.transform.position += transform.up * SPEED * Time.deltaTime;
        }
    }
}
