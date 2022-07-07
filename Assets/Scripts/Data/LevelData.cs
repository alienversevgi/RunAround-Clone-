using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Create LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public List<SquareData> Squares;
    public List<TriangleData> Triangles;
}