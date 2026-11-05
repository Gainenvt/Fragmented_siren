
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinnedMeshBoundsFitter : MonoBehaviour
{
    [SerializeField] private float padding = 1.15f;

    [ContextMenu("Fit Bounds To Character")]
    public void FitBounds()
    {
        SkinnedMeshRenderer skin =
            GetComponent<SkinnedMeshRenderer>();

        if (skin.sharedMesh == null)
        {
            Debug.LogWarning("No mesh assigned!");
            return;
        }

        Bounds bounds = skin.sharedMesh.bounds;

        // Add some extra room for animation.
        bounds.extents *= padding;

        skin.localBounds = bounds;

        Debug.Log("Skinned Mesh Bounds Updated!");
    }
}
