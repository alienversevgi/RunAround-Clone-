using Game.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class MapGenerator : MonoBehaviour
    {
        #region Fields

        private const int SEGMENT_COUNT = 90;
        private const float TAU = 6.28318530718f;
        private const float CENTER_BUFFER_SIZE = 0.5f;
        private const float X_RADIUS = 4;
        private const float Y_RADIUS = 4;

        [SerializeField] private PercentageIndicator _percentageIndicator;
        [SerializeField] private LookAt2D _wallPrefab;
        [SerializeField] private EdgeCollider2D _collider;
        [SerializeField] private Transform _center;

        private List<GameObject> _listOfGameobjects;
        private List<Wall> _walls;
        private List<Vector2> _circlePoints;

        #endregion

        #region Public Methods

        public void SetupCircle()
        {
            _walls = new List<Wall>();
            _listOfGameobjects = new List<GameObject>();

            List<Vector2> circlePoints = GetCirclePoints(SEGMENT_COUNT, X_RADIUS, Y_RADIUS);

            for (int i = 0; i < circlePoints.Count; i++)
            {
                LookAt2D clone = Instantiate(_wallPrefab, circlePoints[i], Quaternion.identity, this.transform);
                clone.name = i.ToString();
                clone.SetTarget(_center);
                _listOfGameobjects.Add(clone.gameObject);
                _walls.Add(clone.GetComponent<Wall>());
            }

            SetCollider();
            SetUpCenterBackground();
        }

        public void UpdateProgress()
        {
            sbyte percentage = GetMapCompletedPercentage();
            _percentageIndicator.SetPercentageText(percentage);

            if (percentage.Equals(100))
            {
                EventManager.Instance.OnLevelCompleted.Raise();
            }
        }

        public void SetCollider()
        {
            List<Vector2> points = GetWalls().Select(it => new Vector2(it.transform.position.x, it.transform.position.y)).ToList(); ;
            points.Add(points[0]);
            _circlePoints = GetCirclePoints(SEGMENT_COUNT, X_RADIUS + .05f, Y_RADIUS + .05f);
            _collider.points = _circlePoints.ToArray();
        }

        public List<Wall> GetWalls() => _walls;

        public List<Vector2> GetCirclePoints() => _circlePoints;

        public void Reset()
        {
            _walls.ForEach(wall => wall.DeactivateRenderer());
            _percentageIndicator.SetPercentageText(0);
        }

        #endregion

        #region Private Methods

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

        private sbyte GetMapCompletedPercentage()
        {
            int activatedCount = GetWalls().Count(it => it.IsActivated);
            return GetPercentage(activatedCount, MapGenerator.SEGMENT_COUNT);
        }

        private sbyte GetPercentage(float current, float max)
        {
            return (sbyte)((current / max) * 100); ;
        }

        private void SetUpCenterBackground()
        {
            float size = (X_RADIUS + Y_RADIUS) + CENTER_BUFFER_SIZE;
            _center.localScale = new Vector2(size, size);
        }

        #endregion
    }
}