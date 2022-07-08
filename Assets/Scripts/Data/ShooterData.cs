using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    [System.Serializable]
    public class ShooterData
    {
        public Vector2 Position;
        public float Speed;
        public DirectionType Direction;
        public float Radiant;
        public float FireRate;
        public float BulletForce;
        public float Angle;
    }
}