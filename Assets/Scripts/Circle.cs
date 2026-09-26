using UnityEngine;

public class CirclePuzzle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CirclePuzzle"))
        {
            Debug.Log("Player collided with the circle puzzle!");
            // Add your logic here for when the player collides with the circle puzzle
            Destroy(collision.gameObject); // Example: Destroy the circle puzzle object
        }
    }
} 