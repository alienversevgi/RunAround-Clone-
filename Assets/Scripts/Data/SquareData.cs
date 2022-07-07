﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    Left,
    Right
}

[System.Serializable]
public class SquareData
{
    public Vector2 Position;
    public float Speed;
    public DirectionType Direction;
    public bool IsMoveable;
}