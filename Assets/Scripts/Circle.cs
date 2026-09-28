using UnityEngine;

public class CirclePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject circlePuzzleObject;
    [SerializeField] private Basesiren sirenEnemy;
    [SerializeField] private MeleeSiren meleeEnemy;
    [SerializeField] private GameObject drag;

    private void Start()
    {
        HideCirclePuzzle();
    }

    private void Update()
    {
        ShowCirclePuzzle();
    }

    private void HideCirclePuzzle()
    {
        circlePuzzleObject.SetActive(false);
        drag.SetActive(false);
    }

    private void ShowCirclePuzzle()
    {
        if (sirenEnemy.isEnemyDead && meleeEnemy.isEnemyDead)
        {
            circlePuzzleObject.SetActive(true);
            drag.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CirclePuzzle"))
        {
            Debug.Log("Player collided with the circle puzzle!");

            Destroy(collision.gameObject);
        }
    }
}