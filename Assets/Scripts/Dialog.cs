
using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text dialogText;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    [SerializeField] private string[] dialogueLines;

    [Header("Timing")]
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        dialogText.gameObject.SetActive(true);

        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        foreach (string line in dialogueLines)
        {
            // Display the current dialogue.
            dialogText.text = line;

            // Make text fully visible.
            SetAlpha(1f);

            // Wait before disappearing.
            yield return new WaitForSeconds(displayDuration);

            // Fade out.
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;

                float alpha = Mathf.Lerp(
                    1f,
                    0f,
                    elapsedTime / fadeDuration
                );

                SetAlpha(alpha);

                yield return null;
            }

            SetAlpha(0f);
        }

        // Hide text when all dialogue is finished.
        dialogText.gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color textColor = dialogText.color;
        textColor.a = alpha;
        dialogText.color = textColor;
    }
}
