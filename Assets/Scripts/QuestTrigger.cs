
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private GameObject questPopup;

    private bool questTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !questTriggered)
        {
            questPopup.SetActive(true);
            questTriggered = true;
        }
    }
}
