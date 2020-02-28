using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Fields

    private const string CircleTag = "Circle";

    // max speed : 5.6
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float jumpForce = 100;

    public Rigidbody2D Rigidbody2D;
    public bool isMovingPlayer;

    private bool doesCollideToCircle;
    private List<Wall> walls;
    private GravityManager gravityManager;

    public Text txt;

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (isMovingPlayer)
        {
            this.transform.position += transform.up * speed * Time.deltaTime;
        }

        float distance = Vector2.Distance(this.transform.position, Vector2.zero);

        Debug.Log(distance);

        if (Input.GetMouseButton(0))
        {
            gravityManager.SetActiveGravity(false);
            Rigidbody2D.AddForce(transform.right * (jumpForce * 100) * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(0))
        {
            gravityManager.SetActiveGravity(true);
        }

        if (distance > 2.0f && distance < 3.0f)
        {
            gravityManager.SetActiveGravity(true);
        }

        txt.text = jumpForce.ToString();
    }

    private void FixedUpdate()
    {
        if (doesCollideToCircle)
        {
            GetNearestWall().ActivateRenderer();
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

    #endregion

    #region Public Methods
    public void Initilize(List<Wall> walls, GravityManager gravityManager)
    {
        this.walls = walls;
        this.gravityManager = gravityManager;
        StartCoroutine(WaitAndPrepareFirstPlay());
    }

    public void IncreaseumpForce()
    {
        jumpForce++;
    }

    public void DecreaseJumpForce()
    {
        jumpForce--;
    }

    #endregion

    #region Private Methods
    private IEnumerator WaitAndPrepareFirstPlay()
    {
        while (!doesCollideToCircle)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        isMovingPlayer = true;
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

    #endregion
}