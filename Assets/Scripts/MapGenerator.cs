using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    private const float TAU = 6.28318530718f;
    private const float CENTER_BUFFER_SIZE = 0.5f;

    [SerializeField] private LookAt2D wallPrefab;

    [SerializeField] private uint segments;
    [SerializeField] private uint xRadius;
    [SerializeField] private uint yRadius;
    [SerializeField] private EdgeCollider2D collider;

    public bool test;
    public bool clear;

    public Transform Center;

    private List<GameObject> listOfGameobjects;
    private List<Wall> walls;

    private void Awake()
    {
        walls = new List<Wall>();
    }

    public void Update()
    {
        if (test)
        {
            SetupCircle();
            test = false;
        }

        if (clear)
        {
            foreach (var item in listOfGameobjects)
            {
                DestroyImmediate(item);
            }
            clear = false;
            listOfGameobjects.Clear();
        }
    }

    public void SetupCircle()
    {
        listOfGameobjects = new List<GameObject>();

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)segments;
            float radian = t * TAU;

            float x = Mathf.Sin(radian) * xRadius;
            float y = Mathf.Cos(radian) * yRadius;
            LookAt2D clone = Instantiate(wallPrefab, new Vector3(x, y), Quaternion.identity, this.transform);
            clone.name = i.ToString();
            clone.SetTarget(Center);
            listOfGameobjects.Add(clone.gameObject);
            walls.Add(clone.GetComponent<Wall>());
        }

        SetCollider();
        SetUpCenterBackground();
    }

    public void SetCollider()
    {
        List<Vector2> points = GetWalls().Select(it => new Vector2(it.transform.position.x, it.transform.position.y)).ToList(); ;
        points.Add(points[0]);
        collider.points = points.ToArray();
    }

    private void SetUpCenterBackground()
    {
        float size = (xRadius + yRadius) + CENTER_BUFFER_SIZE;
        Center.localScale = new Vector2(size, size);
    }

    public List<Wall> GetWalls()
    {
        return walls;
    }

    public uint GetSegmentCount()
    {
        return segments;
    }
}
