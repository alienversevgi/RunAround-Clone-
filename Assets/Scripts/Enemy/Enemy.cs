using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(LookAt2D))]
public abstract class Enemy : MonoBehaviour
{
    private const sbyte COLOR_SWITCH_COUNT = 3;
    private const float COLOR_SWITCH_SECONDS = 0.3f;

    public Rigidbody2D rigidbody { get; protected set; }

    protected Collider2D collider;
    protected SpriteRenderer renderer;
    protected List<Transform> positions;
    protected bool isColorSwitchAnimationFinished;

    private Action gameOverAction;

    public abstract void EnableAction();
    public abstract void DisableAction();

    public virtual void Awake()
    {
        collider = this.GetComponent<Collider2D>();
        renderer = this.GetComponent<SpriteRenderer>();
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    public virtual void Initialize(List<Transform> positions, Action gameOverAction)
    {
        this.positions = positions;
        this.gameOverAction = gameOverAction;
    }

    public virtual void SetPosition(Vector3 position)
    {
        ResetEnemy();
        this.transform.position = position;
        StartCoroutine(ColorSwitch());
    }

    protected Vector3 GetRandomPosition()
    {
        return positions[Random.Range(0, positions.Count)].position;
    }

    public virtual IEnumerator ColorSwitch()
    {
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

    private void ResetEnemy()
    {
        isColorSwitchAnimationFinished = false;
        renderer.color = Color.yellow;
        rigidbody.isKinematic = true;
        collider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.otherCollider.CompareTag("Enemy"))
        {
            gameOverAction();
        }
    }
}