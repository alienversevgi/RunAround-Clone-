using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public Vector3 Position;
    public EnemyType EnemyType;
}

public enum EnemyType
{
    Square,
    Triangle
}