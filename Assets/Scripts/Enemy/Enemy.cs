using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnverPool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(LookAt2D))]
public abstract class Enemy : Entity
{
    private const sbyte COLOR_SWITCH_COUNT = 3;
    private const float COLOR_SWITCH_SECONDS = 0.3f;

    public Rigidbody2D rigidbody { get; protected set; }

    protected Collider2D collider;
    protected SpriteRenderer renderer;
    protected List<Vector2> circlePoints;
    protected bool isColorSwitchAnimationFinished;

    public abstract void EnableAction();
    public abstract void DisableAction();

    public virtual void Awake()
    {
        collider = this.GetComponent<Collider2D>();
        renderer = this.GetComponent<SpriteRenderer>();
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    public void SetCirclePoints(List<Vector2> circlePoints)
    {
        this.circlePoints = circlePoints;
    }

    protected void ShowColorSwitchAnimation()
    {
        isColorSwitchAnimationFinished = false;

        rigidbody.isKinematic = true;
        collider.enabled = false;
        StartCoroutine(ColorSwitch());
    }

    protected Vector2 GetRandomPosition()
    {
        return circlePoints[Random.Range(0, circlePoints.Count)];
    }

    public virtual IEnumerator ColorSwitch()
    {
        renderer.color = Color.yellow;
        for (sbyte i = 0; i < COLOR_SWITCH_COUNT; i++)
        {
            renderer.enabled = false;
            yield return new WaitForSecondsRealtime(COLOR_SWITCH_SECONDS);
            renderer.enabled = true;
            yield return new WaitForSecondsRealtime(COLOR_SWITCH_SECONDS);
        }

        renderer.color = Color.black;
        collider.enabled = true;
        rigidbody.isKinematic = false;
        isColorSwitchAnimationFinished = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.otherCollider.CompareTag("Enemy"))
        {
            DisableAction();
            EventManager.Instance.OnCollideEnemy.Raise();
        }
    }
}