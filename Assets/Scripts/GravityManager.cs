using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityManager : MonoBehaviour
{
    [SerializeField] private float G = 25;

    private Rigidbody2D centerOfRigidbody;
    private List<Rigidbody2D> objects;

    public bool IsGravityActive { get; private set; }

    private void Awake()
    {
        centerOfRigidbody = this.transform.GetComponent<Rigidbody2D>();
        objects = new List<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsGravityActive)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Vector3 dir = (centerOfRigidbody.transform.position - objects[i].transform.position);
                objects[i].AddForce(GetForce(objects[i]) * dir * Time.deltaTime);
            }
        }
    }

    private float GetForce(Rigidbody2D objectOfElement)
    {
        float distance = Vector3.Distance(centerOfRigidbody.transform.position, objectOfElement.transform.position);
        float force = G * ((centerOfRigidbody.mass * objectOfElement.mass) / Mathf.Pow(distance, 2));

        return -force;
    }

    public void SubscribeToGravity(Rigidbody2D rigidbodyOfObject)
    {
        objects.Add(rigidbodyOfObject);
    }

    public void UnsubscribeToGravity(Rigidbody2D rigidbodyOfObject)
    {
        objects.Remove(rigidbodyOfObject);
    }

    public void SetActiveGravity(bool isActive)
    {
        IsGravityActive = isActive;
    }
}