using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;

    [SerializeField] private float speed = 0.1f;

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
    }
}
