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

    [SerializeField] private int segments;
    [SerializeField] private float xRadius;
    [SerializeField] private float yRadius;
    [SerializeField] private EdgeCollider2D collider;

    public bool test;
    public bool clear;

    public Transform Center;

    private List<GameObject> listOfGameobjects;
    private List<Wall> walls;
    private List<Vector2> circlePoints;

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

        List<Vector2> circlePoints = GetCirclePoints(segments, xRadius, yRadius);
        for (int i = 0; i < circlePoints.Count; i++)
        {
            LookAt2D clone = Instantiate(wallPrefab, circlePoints[i], Quaternion.identity, this.transform);
            clone.name = i.ToString();
            clone.SetTarget(Center);
            listOfGameobjects.Add(clone.gameObject);
            walls.Add(clone.GetComponent<Wall>());
        }

        SetCollider();
        SetUpCenterBackground();
    }

    private List<Vector2> GetCirclePoints(int segmentCount, float xRadius, float yRadius)
    {
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)segments;
            float radian = t * TAU;

            float x = Mathf.Sin(radian) * xRadius;
            float y = Mathf.Cos(radian) * yRadius;

            Vector2 point = new Vector2(x, y);
            points.Add(point);
        }

        return points;
    }

    public void SetCollider()
    {
        List<Vector2> points = GetWalls().Select(it => new Vector2(it.transform.position.x, it.transform.position.y)).ToList(); ;
        points.Add(points[0]);
        circlePoints = GetCirclePoints(segments, xRadius + .1f, yRadius + .1f);
        collider.points = circlePoints.ToArray();
    }

    private void SetUpCenterBackground()
    {
        float size = (xRadius + yRadius) + CENTER_BUFFER_SIZE;
        Center.localScale = new Vector2(size, size);
    }

    public List<Wall> GetWalls() => walls;

    public int GetSegmentCount() => segments;

    public List<Vector2> GetCirclePoints() => circlePoints;
}
