
using UnityEngine;
using System.Collections;

public class Floorscripts : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private Basesiren[] sirenEnemy;

    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform floorCamera;
    [SerializeField] private float cameraPanDuration = 2f;

    [Header("Camera Control")]
    [SerializeField] private MonoBehaviour cameraController;

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
        // Stop the player camera controller.
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        // Save original player camera position.
        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        // 1. Move camera to floor camera.
        yield return StartCoroutine(MoveCamera(
            startPosition,
            startRotation,
            floorCamera.position,
            floorCamera.rotation
        ));

        // 2. Return camera to player.
        yield return StartCoroutine(MoveCamera(
            floorCamera.position,
            floorCamera.rotation,
            startPosition,
            startRotation
        ));

        // Restore player camera control.
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        Destroy(gameObject);
    }

    IEnumerator MoveCamera(
        Vector3 startPosition,
        Quaternion startRotation,
        Vector3 targetPosition,
        Quaternion targetRotation)
    {
        float timer = 0f;

        while (timer < cameraPanDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / cameraPanDuration);
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

        playerCamera.position = targetPosition;
        playerCamera.rotation = targetRotation;
    }
}
