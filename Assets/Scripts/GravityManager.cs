using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityManager : MonoBehaviour
{
    [SerializeField] private float G = 10;

    private Rigidbody2D centerOfRigidbody;
    private List<Rigidbody2D> objects;

    private float force;

    private void Awake()
    {
        centerOfRigidbody = this.transform.GetComponent<Rigidbody2D>();
        objects = new List<Rigidbody2D>();
    }

    private void Update()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Vector3 dir = (centerOfRigidbody.transform.position - objects[i].transform.position);
            objects[i].AddForce(-force * dir);
        }
    }

    public void StartGravity()
    {
        Rigidbody2D firstObject = objects.First();
        float distance = Vector3.Distance(centerOfRigidbody.transform.position, firstObject.transform.position);
        force = G * ((centerOfRigidbody.mass * firstObject.mass) / Mathf.Pow(distance, 2));
    }

    public void SubscribeToGravity(Rigidbody2D rigidbodyOfObject)
    {
        objects.Add(rigidbodyOfObject);
    }
}
