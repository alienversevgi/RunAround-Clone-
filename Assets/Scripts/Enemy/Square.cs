using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : Enemy
{
    // = 3.0f
    private float _speed;
    private bool _isMoveable;
    private bool _isEnable;
    private DirectionType _direction;

    public void Initialize(SquareData squareData)
    {
        _speed = squareData.Speed;
        _isMoveable = squareData.IsMoveable;
        _direction = squareData.Direction;
    }

    public override void EnableAction()
    {
        ShowColorSwitchAnimation();
        _isEnable = true;
    }

    public override void DisableAction()
    {
        _isEnable = false;
    }

    private void Update()
    {
        if (!_isMoveable)
            return;

        if (_isEnable && isColorSwitchAnimationFinished)
        {
            int direction = _direction == DirectionType.Right ? 1 : -1;
            this.transform.position += (transform.up * direction) * _speed * Time.deltaTime;
        }
    }
}
