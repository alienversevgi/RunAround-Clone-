using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string CircleTag = "Circle";

    public Rigidbody2D Rigidbody2D;

    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float jumpForce = 100;

    [SerializeField] private MapGenerator mapGenerator;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            this.transform.position += transform.up * speed;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.position -= transform.up * speed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D.AddForce(jumpForce * transform.right * Time.deltaTime);
        }
    }

    private Wall GetNearestWall()
    {
        List<Wall> walls = mapGenerator.GetWalls();
        Wall selectedWall = walls.First();
        float minPosition = Vector3.Distance(this.transform.position, walls.First().transform.position);

        foreach (Wall wall in walls)
        {
            float currentPosition = Vector3.Distance(this.transform.position, wall.transform.position);
            if (currentPosition < minPosition)
            {
                minPosition = currentPosition;
                selectedWall = wall;
            }
        }

        return selectedWall;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(CircleTag))
        {
            GetNearestWall().ActivateRenderer();
        }
    }
}
