using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        private const string CIRCLE_TAG = "Circle";
        private const float MIN_JUMPFORCE = 3.46f;

        [SerializeField] private float _speed = 4.0f;
        [SerializeField] private float _jumpForce = 35.0f;

        public Rigidbody2D Rigidbody2D;

        private Vector2 _defaultStartPosition = new Vector2(0.0f, -3.8f);
        private List<Wall> _walls;
        private GravityManager _gravityManager;
        private Coroutine _airWaitCouritine;

        private bool _doesCollideToCircle;
        private bool _isMovingPlayer;
        private bool _isResetRequiring;
        private bool _isControllerEnable;
        private float _distance;
        private bool _isFirstInputDetected;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (!_isFirstInputDetected && Input.GetMouseButton(0))
            {
                _isFirstInputDetected = true;
                EventManager.Instance.OnFirstInputDetected.Raise();
            }

            if (!_isControllerEnable)
                return;

            if (_doesCollideToCircle)
            {
                Wall wall = GetNearestWall();

                if (!wall.IsActivated)
                {
                    wall.ActivateRenderer();
                    EventManager.Instance.OnProgressIncreased.Raise();
                }
            }

            if (_isMovingPlayer)
                this.transform.position += transform.up * _speed * Time.deltaTime;

            CheckJumpProgress();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(CIRCLE_TAG))
                _doesCollideToCircle = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(CIRCLE_TAG))
                _doesCollideToCircle = false;
        }

        #endregion

        #region Public Methods

        public void Initilize(List<Wall> walls, GravityManager gravityManager)
        {
            this._walls = walls;
            this._gravityManager = gravityManager;
            this.transform.position = _defaultStartPosition;
            _isMovingPlayer = true;
            _isFirstInputDetected = false;
        }


        public void SetActiveController(bool isActive)
        {
            _isControllerEnable = isActive;
            _isFirstInputDetected = isActive;
            this.transform.position = _defaultStartPosition;
        }

        #endregion

        #region Private Methods

        private void CheckJumpProgress()
        {
            _distance = Vector2.Distance(this.transform.position, Vector2.zero);

            if (_doesCollideToCircle && Input.GetMouseButtonDown(0))
            {
                _isResetRequiring = false;
            }

            if (!_isResetRequiring && Input.GetMouseButton(0))
            {
                Jump();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isResetRequiring = true;
                StartCoroutine(ForceCoroutine(MIN_JUMPFORCE));
            }
        }

        private void Jump()
        {
            if (IsPlayerInAir())
            {
                if (_airWaitCouritine == null)
                {
                    _gravityManager.SetActiveGravity(false);
                    Rigidbody2D.velocity = Vector2.zero;
                    Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

                    //wait a second in the air
                    _airWaitCouritine = Utility.Timer.Instance.StartTimer(.1f, () =>
                    {
                        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        _gravityManager.SetActiveGravity(true);
                        _isResetRequiring = true;
                        _airWaitCouritine = null;
                    });
                }
            }
            else
            {
                _gravityManager.SetActiveGravity(false);
                Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                Rigidbody2D.AddForce(transform.right * (_jumpForce * 100) * Time.deltaTime);
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
            Wall selectedWall = _walls.First();
            float minPosition = Vector3.Distance(this.transform.position, _walls.First().transform.position);

            foreach (Wall wall in _walls)
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

            while (_distance > targetDistance)
            {
                Rigidbody2D.AddForce(transform.right * (_jumpForce * 100) * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            _gravityManager.SetActiveGravity(true);
        }

        #endregion
    }
}