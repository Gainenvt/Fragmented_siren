
using UnityEngine;
using TMPro;

public class ObjectIdentifier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text objectText;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 5f;

    private void Update()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            ObjectPrompt prompt =
                hit.collider.GetComponentInParent<ObjectPrompt>();

            if (prompt != null)
            {
                objectText.text = prompt.DisplayName;
                objectText.gameObject.SetActive(true);
                return;
            }
        }

        objectText.gameObject.SetActive(false);
    }
}
