
using UnityEngine;
using UnityEngine.Events;

public class ObjectPrompt : MonoBehaviour
{
    [Header("Object Information")]
    [SerializeField] private string displayName;

    [Header("Identifier Settings")]
    [SerializeField] private bool canIdentify = true;
    [SerializeField] private bool canInteract = true;

    [Header("Interaction")]
    [SerializeField] private UnityEvent onInteract;

    public string DisplayName => displayName;
    public bool CanIdentify => canIdentify;
    public bool CanInteract => canInteract;

    public void Interact()
    {
        if (!canInteract) return;

        onInteract.Invoke();
    }
}
