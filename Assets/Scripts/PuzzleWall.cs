
using UnityEngine;

public class PuzzleWall : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private string requiredObjectName;

    [Header("References")]
    [SerializeField] private PuzzleManager puzzleManager;

    public bool IsActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (IsActivated) return;

        // Find ObjectPrompt, including on parent objects.
        ObjectPrompt prompt = other.GetComponentInParent<ObjectPrompt>();

        if (prompt == null) return;

        // Check whether this is the correct object.
        if (prompt.DisplayName != requiredObjectName) return;

        IsActivated = true;

        Debug.Log("Puzzle wall activated by: " + prompt.DisplayName);

        // Remove the accepted object.
        prompt.gameObject.SetActive(false);

        // Check whether both walls are activated.
        if (puzzleManager != null)
        {
            puzzleManager.CheckPuzzle();
        }
    }
}
