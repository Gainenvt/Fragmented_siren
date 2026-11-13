using UnityEngine;

public class CityKey : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] private GameObject[] towerPartsToDestroy;

    [Header("Distant Towers")]
    [SerializeField] private GameObject[] distantTowers;

    private bool isActivated = false;

    public void ActivateCityKey()
    {
        if (isActivated) return;

        isActivated = true;

        // Destroy the upper tower sections
        foreach (GameObject part in towerPartsToDestroy)
        {
            if (part != null)
            {
                Destroy(part);
            }
        }

        // Reveal distant towers
        foreach (GameObject tower in distantTowers)
        {
            if (tower != null)
            {
                tower.SetActive(true);
            }
        }

        Debug.Log("City Key activated!");
    }
}