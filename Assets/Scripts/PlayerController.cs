using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string CircleTag = "Circle";

    // max speed : 5.6
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float jumpForce = 100;
    [SerializeField] private MapGenerator mapGenerator;

    public Rigidbody2D Rigidbody2D;
    public bool isMovingPlayer;

    private bool doesCollideToCircle;
    private List<Wall> walls;

    private IEnumerator Start()
    {
        walls = mapGenerator.GetWalls();
        while (!doesCollideToCircle)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        isMovingPlayer = true;
    }

    private void Update()
    {
        if (isMovingPlayer)
        {
            this.transform.position += transform.up * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D.AddForce(jumpForce * transform.right * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (doesCollideToCircle)
        {
            GetNearestWall().ActivateRenderer();
        }
    }

    private Wall GetNearestWall()
    {
        Wall selectedWall = walls.First();
        float minPosition = Vector3.Distance(this.transform.position, walls.First().transform.position);

        foreach (Wall wall in walls)
        {
            float currentPosition = Vector3.Distance(this.transform.position, wall.transform.position);
            Mathf.Min(currentPosition, minPosition);
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

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(CircleTag))
            doesCollideToCircle = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(CircleTag))
            doesCollideToCircle = false;
    }
}
