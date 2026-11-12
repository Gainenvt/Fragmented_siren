
using UnityEngine;
using System.Collections;

public class Floorscripts : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private Basesiren[] sirenEnemy;

    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform floorCameraTarget;
    [SerializeField] private float cameraPanDuration = 2f;

    [Header("Floor Animation")]
    [SerializeField] private Transform floor;
    [SerializeField] private float shrinkDuration = 2f;

    private bool sequenceStarted = false;

    void Update()
    {
        if (!sequenceStarted)
        {
            destroyfloor();
        }
    }

    private void destroyfloor()
    {
        for (int i = 0; i < sirenEnemy.Length; i++)
        {
            if (sirenEnemy[i] != null && !sirenEnemy[i].isEnemyDead)
            {
                return;
            }
        }

        sequenceStarted = true;
        StartCoroutine(FloorDestructionSequence());
    }

    IEnumerator FloorDestructionSequence()
    {
        // 1. Pan camera towards floor.
        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        Vector3 targetPosition = floorCameraTarget.position;
        Quaternion targetRotation = floorCameraTarget.rotation;

        float timer = 0f;

        while (timer < cameraPanDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / cameraPanDuration);

            // Smooth camera movement.
            t = Mathf.SmoothStep(0f, 1f, t);

            playerCamera.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            playerCamera.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                t
            );

            yield return null;
        }

        // 2. Shrink the floor.
        Vector3 originalScale = floor.localScale;

        timer = 0f;

        while (timer < shrinkDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / shrinkDuration);

            t = Mathf.SmoothStep(0f, 1f, t);

            floor.localScale = Vector3.Lerp(
                originalScale,
                Vector3.zero,
                t
            );

            yield return null;
        }

        // 3. Destroy the floor.
        Destroy(gameObject);
    }
}
