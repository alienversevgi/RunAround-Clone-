using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private SpriteRenderer renderer;
    public bool IsActivated => renderer.enabled;

    private void Awake()
    {
        renderer = this.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }
    private void Update()
    {
        Vector3 dir = this.transform.parent.position - this.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ActivateRenderer()
    {
        renderer.enabled = true;
        this.name += " Cleared";
    }

    private IEnumerator WaitAndHideDisplay()
    {
        StopCoroutine(WaitAndHideDisplay());
        yield return new WaitForSecondsRealtime(2.0f);

        renderer.enabled = false;
    }
}
