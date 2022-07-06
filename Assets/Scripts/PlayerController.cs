using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Fields

    private const string CIRCLE_TAG = "Circle";
    private const float MIN_JUMPFORCE = 3.46f;

    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float jumpForce = 20;

    public Rigidbody2D Rigidbody2D;

    public event Action ProgressIncreased;

    private List<Wall> walls;
    private GravityManager gravityManager;
    private bool doesCollideToCircle;
    private bool isMovingPlayer;
    private bool isResetRequiring;
    private bool isControllerEnable = true;
    private float distance;
    
    #endregion

    #region Unity Methods

    private void Update()
    {
        if (!isControllerEnable)
            return;

        if (doesCollideToCircle)
        {
            Wall wall = GetNearestWall();

            if (!wall.IsActivated)
            {
                wall.ActivateRenderer();
                ProgressIncreased();
            }
        }

        if (isMovingPlayer)
            this.transform.position += transform.up * speed * Time.deltaTime;

        CheckJumpProgress();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(CIRCLE_TAG))
            doesCollideToCircle = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(CIRCLE_TAG))
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

    public void StopPlayer()
    {
        isControllerEnable = false;
    }

    #endregion

    #region Private Methods

    private void CheckJumpProgress()
    {
        distance = Vector2.Distance(this.transform.position, Vector2.zero);

        if (doesCollideToCircle && Input.GetMouseButtonDown(0))
        {
            isResetRequiring = false;
        }

        if (!isResetRequiring && Input.GetMouseButton(0))
        {
            if (distance < 2.8f)
            {
                gravityManager.SetActiveGravity(true);
                isResetRequiring = true;
            }
            else
            {
                gravityManager.SetActiveGravity(false);
                Rigidbody2D.AddForce(transform.right * (jumpForce * 100) * Time.deltaTime);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isResetRequiring = true;
            StartCoroutine(ForceCoroutine(MIN_JUMPFORCE));
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

    private IEnumerator ForceCoroutine(float targetDistance)
    {
        StopCoroutine(ForceCoroutine(targetDistance));

        while (distance > targetDistance)
        {
            Rigidbody2D.AddForce(transform.right * (jumpForce * 100) * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        gravityManager.SetActiveGravity(true);
    }

    #endregion
}