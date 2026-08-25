using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class HostileAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Layers")]
    [SerializeField] private LayerMask Terrain;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool hasPatrolPoint;

    [Header("Combat Settings")]
    [SerializeField] private float AttackCooldown = 1f;
    private bool isOnAttackCooldown;

    [SerializeField] private float forwardShotForce = 10f;
    [SerializeField] private float verticalShotForce = 5f;

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;

    [Header("Move and Attack")]
    [SerializeField] private float moveSpeed = 3f;
    private bool isPlayerVisible;
    private bool isPlayerInRange;
    private Rigidbody rb;

   private void Awake()
{
    if (playerTransform == null)
    {
        GameObject playerObj = GameObject.Find("Player");

        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

  

    rb = GetComponent<Rigidbody>();
}

private void Update()
{
    DetectPlayer();
    UpdateBehaviourState();
} 
    private void UpdateBehaviourState()
{
    if (!isPlayerVisible)
    {
        FindPatrolPath();
    }
    else if (!isPlayerInRange)
    {
        FetchHIM();
    }
    else
    {
        GunhimDOWN();
    }
}
    


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, engagementRange);
    }


    private void DetectPlayer()
    {
        isPlayerVisible = Physics.CheckSphere(
            transform.position,
            visionRange,
            playerLayerMask
        );

        isPlayerInRange = Physics.CheckSphere(
            transform.position,
            engagementRange,
            playerLayerMask
        );

        Debug.Log("Player Visible: " + isPlayerVisible);
        Debug.Log("Player In Range: " + isPlayerInRange);
    }


    private void FireProjectile()
{
    if (projectilePrefab == null || firePoint == null)
        return;

    Rigidbody projectileRb = Instantiate(
        projectilePrefab,
        firePoint.position,
        firePoint.rotation
    ).GetComponent<Rigidbody>();

    projectileRb.AddForce(
        transform.forward * forwardShotForce,
        ForceMode.Impulse
    );

    projectileRb.AddForce(
        transform.up * verticalShotForce,
        ForceMode.Impulse
    );

    Destroy(projectileRb.gameObject, 3f);
}

    private void FindPatrolPoint()
{
    float randomX = Random.Range(-patrolRadius, patrolRadius);
    float randomY = Random.Range(-patrolRadius, patrolRadius);
    float randomZ = Random.Range(-patrolRadius, patrolRadius);

    currentPatrolPoint = transform.position + new Vector3(
        randomX,
        randomY,
        randomZ
    );

    hasPatrolPoint = true;
}

    private IEnumerator AttackcooldownRoutine()
    {
        isOnAttackCooldown = true;
        yield return new WaitForSeconds(AttackCooldown);
        isOnAttackCooldown = false;
    }

   private void FindPatrolPath()
{
    if (!hasPatrolPoint)
    {
        FindPatrolPoint();
    }

    Vector3 direction = (currentPatrolPoint - transform.position).normalized;

    rb.linearVelocity = direction * moveSpeed;

    if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
    {
        hasPatrolPoint = false;
        rb.linearVelocity = Vector3.zero;
    }
}
   private void FetchHIM()
{
    if (playerTransform != null)
    {
        Vector3 direction =
            (playerTransform.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }
}
    
    private void GunhimDOWN()
{
    if (isPlayerInRange)
    {
        rb.linearVelocity = Vector3.zero;

        if (playerTransform != null)
        {
            transform.LookAt(playerTransform);
        }

        if (!isOnAttackCooldown)
        {
            FireProjectile();
            StartCoroutine(AttackcooldownRoutine());
        }
    }
}


}