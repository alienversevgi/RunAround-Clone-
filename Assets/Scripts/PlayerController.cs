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

    private Vector2 defaultStartPosition = new Vector2(0.0f, -3.8f);
    private List<Wall> walls;
    private GravityManager gravityManager;
    private Coroutine airWaitCouritine;

    private bool doesCollideToCircle;
    private bool isMovingPlayer;
    private bool isResetRequiring;
    private bool isControllerEnable;
    private float distance;
    private bool isFirstInputDetected;

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (!isFirstInputDetected && Input.GetMouseButton(0))
        {
            isFirstInputDetected = true;
            EventManager.Instance.OnFirstInputDetected.Raise();
        }

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
        this.transform.position = defaultStartPosition;
        isMovingPlayer = true;
        isFirstInputDetected = false;
    }


    public void SetActiveController(bool isActive)
    {
        isControllerEnable = isActive;
        isFirstInputDetected = isActive;
        this.transform.position = defaultStartPosition;
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
            Jump();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isResetRequiring = true;
            StartCoroutine(ForceCoroutine(MIN_JUMPFORCE));
        }
    }

    private void Jump()
    {
        if (IsPlayerInAir())
        {
            if (airWaitCouritine == null)
            {
                gravityManager.SetActiveGravity(false);
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

                //wait a second in the air
                airWaitCouritine = Utility.Timer.Instance.StartTimer(.1f, () =>
                {
                    Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    gravityManager.SetActiveGravity(true);
                    isResetRequiring = true;
                    airWaitCouritine = null;
                });
            }
        }
        else
        {
            gravityManager.SetActiveGravity(false);
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            Rigidbody2D.AddForce(transform.right * (jumpForce * 100) * Time.deltaTime);
        }
    }

    private bool IsPlayerInAir()
    {
        Ray2D ray;
        int wallLayerMask = LayerMask.GetMask("Circle");

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -this.transform.right, 10, wallLayerMask);

        bool isPlayerInAir = hit.distance > 2.0f && hit.distance < 2.5f;

        return isPlayerInAir;
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