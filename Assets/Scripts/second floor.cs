using UnityEngine;

public class secondfloor : MonoBehaviour
{
    [SerializeField] private GameObject Floor;
    [SerializeField] private MeleeSiren[] BaseSiren;

     void Update()
    {
        destroyfloor();
    }

       private void destroyfloor()
    {
        for (int i = 0; i < BaseSiren.Length; i++)
        {
            if (!BaseSiren[i].isEnemyDead)
            {
                return;
            }
        }

        Destroy(gameObject);
    }
}
