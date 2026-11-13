
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Walls")]
    [SerializeField] private PuzzleWall wall1;
    [SerializeField] private PuzzleWall wall2;

    [Header("Floor")]
    [SerializeField] private GameObject[] floorsAbove;
    private bool puzzleComplete = false;

    public void CheckPuzzle()
    {
        if (puzzleComplete) return;

        if (wall1.IsActivated && wall2.IsActivated)
        {
            puzzleComplete = true;

            Debug.Log("Both puzzle walls activated!");

            foreach (GameObject floor in floorsAbove)
            {
                floor.SetActive(false);
            }
        }
    }
}
