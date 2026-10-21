using UnityEngine;

public class secondfloor : MonoBehaviour
{
    [SerializeField] private GameObject Floor;
    [SerializeField] private string targetTag = "Melee_Enemy";

    private Basesiren[] meleeEnemies;

    private void Start()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(targetTag);

        meleeEnemies = new Basesiren[enemyObjects.Length];

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            meleeEnemies[i] = enemyObjects[i].GetComponent<Basesiren>();
        }
    }

    private void Update()
    {
        DestroyFloor();
    }

    private void DestroyFloor()
    {
        // Don't destroy the floor if no enemies were found.
        if (meleeEnemies == null || meleeEnemies.Length == 0)
            return;

        foreach (Basesiren enemy in meleeEnemies)
        {
            // An enemy without the script should not count as defeated.
            if (enemy == null)
                return;

            if (!enemy.isEnemyDead)
                return;
        }

        Destroy(gameObject);
    }
}