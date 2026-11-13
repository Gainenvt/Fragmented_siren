
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
        // 1. Stop player camera movement
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        // 2. Save the camera's original position
        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        // Also save its position relative to the Player.
        Vector3 originalLocalPosition = playerCamera.localPosition;
        Quaternion originalLocalRotation = playerCamera.localRotation;

        // 3. Move camera to the floor camera
        yield return StartCoroutine(MoveCamera(
            startPosition,
            startRotation,
            floorCamera.position,
            floorCamera.rotation
        ));

        // 4. Return camera to its original world position
        yield return StartCoroutine(MoveCamera(
            floorCamera.position,
            floorCamera.rotation,
            startPosition,
            startRotation
        ));

        // 5. Restore the original local position and rotation
        playerCamera.localPosition = originalLocalPosition;
        playerCamera.localRotation = originalLocalRotation;

        // 6. Give camera control back to the player
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        // 7. Destroy the floor
        Destroy(gameObject);

        Debug.Log("Floor destroyed and camera returned to player.");
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
