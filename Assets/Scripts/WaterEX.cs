using UnityEngine;
using System.Collections;

public class WaterRipple : MonoBehaviour
{
    public float duration = 0.8f;
    public float maxScale = 4f;

    private Vector3 startingScale;

    void Start()
    {
        startingScale = transform.localScale;
        StartCoroutine(Expand());
    }

    IEnumerator Expand()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            transform.localScale = Vector3.Lerp(
                startingScale,
                startingScale * maxScale,
                t
            );

            yield return null;
        }

        Destroy(gameObject);
    }
}