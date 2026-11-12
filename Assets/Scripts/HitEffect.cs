
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitEffect : MonoBehaviour
{
    public Image hitImage;
    public float fadeSpeed = 2f;
    public float maxAlpha = 0.6f;
  
    private Coroutine hitCoroutine;

    void Start()
    {
        hitImage.color = new Color(1, 1, 1, 0);
    }

    public void ShowHitEffect()
    {

        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        hitCoroutine = StartCoroutine(FadeEffect());
    }

    IEnumerator FadeEffect()
    {
        hitImage.color = new Color(1, 1, 1, maxAlpha);

        while (hitImage.color.a > 0.01f)
        {
            Color currentColor = hitImage.color;

            currentColor.a = Mathf.MoveTowards(
                currentColor.a,
                0,
                fadeSpeed * Time.deltaTime
            );

            hitImage.color = currentColor;

            yield return null;
        }

        hitImage.color = new Color(1, 1, 1, 0);
        hitCoroutine = null;
    }
}
