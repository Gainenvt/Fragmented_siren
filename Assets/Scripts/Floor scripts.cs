using UnityEngine;

public class Floorscripts : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject Floor;
    [SerializeField] private Basesiren[] sirenEnemy;
    private Basesiren basesiren;


    void Start()
    {
        
    }       

    // Update is called once per frame
    void Update()
    {
        destroyfloor();
    }

       private void destroyfloor()
    {
        for (int i = 0; i < sirenEnemy.Length; i++)
        {
            if (!sirenEnemy[i].isEnemyDead)
            {
                return;
            }
        }

        Destroy(gameObject);
    }

}
