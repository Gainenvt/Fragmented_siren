using UnityEngine;
using TMPro;

public class InstructionText : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float displayTime = 5f;
    public float fadeSpeed = 2f;

    private void Start()
    {
        canvasGroup.alpha = 1f;
        Invoke(nameof(FadeOut), displayTime);
    }

    private void FadeOut()
    {
        StartCoroutine(Fade());
    }

    private System.Collections.IEnumerator Fade()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}