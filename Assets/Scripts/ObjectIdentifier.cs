
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ObjectIdentifier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text objectText;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 5f;

    private ObjectPrompt currentPrompt;

    private void Update()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        currentPrompt = null;

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            ObjectPrompt prompt =
                hit.collider.GetComponentInParent<ObjectPrompt>();

            if (prompt != null && prompt.CanIdentify)
            {
                currentPrompt = prompt;

                objectText.text = prompt.DisplayName;

                if (prompt.CanInteract)
                {
                    objectText.text += "\n[E] Interact";
                }

                objectText.gameObject.SetActive(true);

                // New Input System
                if (prompt.CanInteract &&
                    Keyboard.current != null &&
                    Keyboard.current.eKey.wasPressedThisFrame)
                {
                    prompt.Interact();
                }

                return;
            }
        }

        objectText.gameObject.SetActive(false);
    }
}
