using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private SpriteRenderer renderer;

    private void Awake()
    {
        renderer = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 dir = this.transform.parent.position - this.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            renderer.enabled = true;
            StartCoroutine(WaitAndHideDisplay());
        }       
    }

    private IEnumerator WaitAndHideDisplay()
    {
        StopCoroutine(WaitAndHideDisplay());
        yield return new WaitForSecondsRealtime(2.0f);

        renderer.enabled = false;
    }

}
