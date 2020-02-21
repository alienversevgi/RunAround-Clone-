using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private uint segments;
    [SerializeField] private uint xRadius;
    [SerializeField] private uint yRadius;

    [SerializeField] private Wall[] walls;
    [SerializeField] private PlayerController player;

    [SerializeField] private GravityManager gravityManager;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        DrawCircle();
        gravityManager.SubscribeToGravity(player.Rigidbody2D);
        gravityManager.StartGravity();
    }

    private void DrawCircle()
    {
        walls = this.transform.GetComponentsInChildren<Wall>();
        float angle = 20;

        for (int i = 0; i < walls.Length; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;
            walls[i].transform.position = new Vector3(x, y);

            angle += (360 / segments);
        }
    }
}