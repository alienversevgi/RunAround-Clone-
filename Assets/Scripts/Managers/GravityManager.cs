using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float G = 100;

        private Rigidbody2D _centerOfRigidbody;
        private List<Rigidbody2D> _objects;

        public bool IsGravityActive { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _centerOfRigidbody = this.transform.GetComponent<Rigidbody2D>();
            _objects = new List<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (IsGravityActive)
            {
                for (int i = 0; i < _objects.Count; i++)
                {
                    Vector3 dir = (_centerOfRigidbody.transform.position - _objects[i].transform.position);
                    _objects[i].AddForce(GetForce(_objects[i]) * dir * Time.deltaTime);
                }
            }
        }

        #endregion

        #region Public Methods

        public void SubscribeToGravity(Rigidbody2D rigidbodyOfObject)
        {
            _objects.Add(rigidbodyOfObject);
        }

        public void UnsubscribeToGravity(Rigidbody2D rigidbodyOfObject)
        {
            _objects.Remove(rigidbodyOfObject);
        }

        public void SetActiveGravity(bool isActive)
        {
            IsGravityActive = isActive;
        }

        #endregion

        #region Private Methods

        private float GetForce(Rigidbody2D objectOfElement)
        {
            float distance = Vector3.Distance(_centerOfRigidbody.transform.position, objectOfElement.transform.position);
            float force = G * ((_centerOfRigidbody.mass * objectOfElement.mass) / Mathf.Pow(distance, 2));

            return -force;
        }

        #endregion
    }
}