using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    private const int SEGMENT_COUNT = 90;
    private const float TAU = 6.28318530718f;
    private const float CENTER_BUFFER_SIZE = 0.5f;
    private const float X_RADIUS = 4;
    private const float Y_RADIUS = 4;

    [SerializeField] private PercentageIndicator percentageIndicator;
    [SerializeField] private LookAt2D wallPrefab;
    [SerializeField] private EdgeCollider2D collider;

    public Transform Center;

    private List<GameObject> listOfGameobjects;
    private List<Wall> walls;
    private List<Vector2> circlePoints;

    public void SetupCircle()
    {
        walls = new List<Wall>();
        listOfGameobjects = new List<GameObject>();

        List<Vector2> circlePoints = GetCirclePoints(SEGMENT_COUNT, X_RADIUS, Y_RADIUS);

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
            float t = i / (float)SEGMENT_COUNT;
            float radian = t * TAU;

            float x = Mathf.Sin(radian) * xRadius;
            float y = Mathf.Cos(radian) * yRadius;

            Vector2 point = new Vector2(x, y);
            points.Add(point);
        }

        return points;
    }


    public void UpdateProgress()
    {
        sbyte percentage = GetMapCompletedPercentage();
        percentageIndicator.SetPercentageText(percentage);
        
        if (percentage.Equals(100))
        {
            EventManager.Instance.OnLevelCompleted.Raise();
        }
    }

    public void Reset()
    {
        walls.ForEach(wall => wall.DeactivateRenderer());
        percentageIndicator.SetPercentageText(0);
    }

    private sbyte GetMapCompletedPercentage()
    {
        int activatedCount = GetWalls().Count(it => it.IsActivated);
        return GetPercentage(activatedCount, MapGenerator.SEGMENT_COUNT);
    }

    private sbyte GetPercentage(float current, float max)
    {
        return (sbyte)((current / max) * 100); ;
    }

    public void SetCollider()
    {
        List<Vector2> points = GetWalls().Select(it => new Vector2(it.transform.position.x, it.transform.position.y)).ToList(); ;
        points.Add(points[0]);
        circlePoints = GetCirclePoints(SEGMENT_COUNT, X_RADIUS + .05f, Y_RADIUS + .05f);
        collider.points = circlePoints.ToArray();
    }

    private void SetUpCenterBackground()
    {
        float size = (X_RADIUS + Y_RADIUS) + CENTER_BUFFER_SIZE;
        Center.localScale = new Vector2(size, size);
    }

    public List<Wall> GetWalls() => walls;

    public List<Vector2> GetCirclePoints() => circlePoints;
}
