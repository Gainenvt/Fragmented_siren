
using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text dialogText;

    [Header("Dialogue Sequences")]
    [TextArea(2, 5)]
    [SerializeField] private string[] towerEntryLines;

    [TextArea(2, 5)]
    [SerializeField] private string[] puzzleCompleteLines;

    [TextArea(2, 5)]
    [SerializeField] private string[] cityKeyLines;

    [TextArea(2, 5)]
    [SerializeField] private string[] endingLines;

    [Header("Timing")]
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeDuration = 1.5f;

    private Coroutine dialogueCoroutine;

    public enum DialogueSequence
    {
        TowerEntry,
        PuzzleComplete,
        CityKey,
        Ending
    }

    private void Start()
    {
        dialogText.gameObject.SetActive(true);
    }

    public void PlayDialogue(DialogueSequence sequence)
    {
        Debug.Log("1. PlayDialogue was called!");
        Debug.Log("4. Dialogue coroutine started!");

        dialogText.enabled = true;

        Debug.Log("5. TMP enabled: " + dialogText.enabled);

        string[] lines = sequence switch
        {
            DialogueSequence.TowerEntry => towerEntryLines,
            DialogueSequence.PuzzleComplete => puzzleCompleteLines,
            DialogueSequence.CityKey => cityKeyLines,
            DialogueSequence.Ending => endingLines,
            _ => null
        };

        Debug.Log("2. Sequence selected: " + sequence);

        if (lines == null || lines.Length == 0)
        {
            Debug.LogError("3. NO DIALOGUE LINES FOUND!");
            return;
        }

        Debug.Log("3. Found " + lines.Length + " dialogue lines!");

        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueCoroutine = StartCoroutine(PlayDialogueRoutine(lines));
    }

    private IEnumerator PlayDialogueRoutine(string[] lines)
    {
        dialogText.gameObject.SetActive(true);

        foreach (string line in lines)
        {
            dialogText.text = line;
            SetAlpha(1f);

            yield return new WaitForSeconds(displayDuration);

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

        dialogText.gameObject.SetActive(true);
        dialogueCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color textColor = dialogText.color;
        textColor.a = alpha;
        dialogText.color = textColor;
    }
}
