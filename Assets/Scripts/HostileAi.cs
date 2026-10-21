using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class HostileAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator animator;
    [Header("Layers")]
    [SerializeField] private LayerMask Terrain;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool hasPatrolPoint;
private bool isSwimming;

    [Header("Combat Settings")]
    [SerializeField] private float AttackCooldown = 1f;
    private bool isOnAttackCooldown;

    [SerializeField] private float forwardShotForce = 10f;

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
            playerTransform = playerObj.transform;
    }

    if (animator == null)
        animator = GetComponentInChildren<Animator>();

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

private void SetSwimming(bool swimming)
{
    if (animator != null)
        animator.SetBool("IsSwiming", swimming);
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
    
      
    }



private void FireProjectile()
{
    if (projectilePrefab == null || firePoint == null || playerTransform == null)
        return;

    // Calculate the direction from the FirePoint to the player
    Vector3 direction = (playerTransform.position - firePoint.position).normalized;

    // Spawn the projectile facing the player
    GameObject projectile = Instantiate(
        projectilePrefab,
        firePoint.position,
        Quaternion.LookRotation(direction)
    );

    // Apply force in the direction the projectile is facing
    Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

    if (projectileRb != null)
    {
        projectileRb.AddForce(
            direction * forwardShotForce,
            ForceMode.Impulse
        );
    }

    Destroy(projectile, 3f);
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
    if (rb == null)
        return;

    if (!hasPatrolPoint)
        FindPatrolPoint();

    Vector3 direction =
        (currentPatrolPoint - transform.position).normalized;

    rb.linearVelocity = direction * moveSpeed;

    SetSwimming(true);

    if (direction.sqrMagnitude > 0.001f)
        transform.rotation = Quaternion.LookRotation(direction);

    if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
    {
        hasPatrolPoint = false;
        rb.linearVelocity = Vector3.zero;
        SetSwimming(false);
    }
}   private void FetchHIM()
{
    if (playerTransform == null || rb == null)
        return;

    Vector3 direction =
        (playerTransform.position - transform.position).normalized;

    rb.linearVelocity = direction * moveSpeed;

    SetSwimming(true);
    FacePlayer();
    
}
private void FacePlayer()
{
    if (playerTransform == null)
        return;

    Vector3 direction =
        playerTransform.position - transform.position;

    if (direction.sqrMagnitude > 0.001f)
        transform.rotation = Quaternion.LookRotation(direction);
}
    
    private void GunhimDOWN()
{
    if (!isPlayerInRange || rb == null)
        return;

    rb.linearVelocity = Vector3.zero;
    SetSwimming(false);
    FacePlayer();

    if (!isOnAttackCooldown)
    {
        FireProjectile();
        StartCoroutine(AttackcooldownRoutine());
    }
}


}