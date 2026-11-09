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

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;

    [Header("Move and Attack")]
    [SerializeField] private float moveSpeed = 3f;
    private bool isPlayerVisible;
    private bool isPlayerInRange;
    private Rigidbody rb;
    private EnemyAnimation enemyAnimation;

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

    enemyAnimation = GetComponent<EnemyAnimation>();

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

    if (projectileRb == null)
        return;

    Vector3 direction =
        (playerTransform.position - firePoint.position).normalized;

    projectileRb.AddForce(
        direction * forwardShotForce,
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
   
enemyAnimation.animator.SetBool("isIdle", true);
enemyAnimation.animator.SetBool("isSwimming", false);
enemyAnimation.animator.SetBool("isAttacking", false);

}
   private void FetchHIM()
{
    if (playerTransform != null)
    {
        Vector3 direction =
            (playerTransform.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }
    enemyAnimation.animator.SetBool("isSwimming", true);
    enemyAnimation.animator.SetBool("isAttacking", false);
    enemyAnimation.animator.SetBool("isIdle", false);
}
    
    private void GunhimDOWN()
{
    if (isPlayerInRange)
    {
        rb.linearVelocity = Vector3.zero;

        enemyAnimation.animator.SetBool("isAttacking", true);
        enemyAnimation.animator.SetBool("isSwimming", false);
        enemyAnimation.animator.SetBool("isIdle", false);

        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0f; // Keep the enemy upright

             if (direction.sqrMagnitude > 0.001f)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
        }

        if (!isOnAttackCooldown)
        {
            FireProjectile();
            StartCoroutine(AttackcooldownRoutine());
        }
    }
    
}


} 