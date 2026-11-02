using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class MeleeHostileAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    public Animator Animator;
    [Header("Layers")]
    [SerializeField] private LayerMask Terrain;
    [SerializeField] private LayerMask playerLayerMask;
    private EnemyAnimation enemyAnimation;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool hasPatrolPoint;
    public bool isSwimming;
    public bool isAttacking;

    [Header("Combat Settings")]
    [SerializeField] private float AttackCooldown = 1f;
    private bool isOnAttackCooldown;


    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 1f;

    [Header("Move and Attack")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float meleeRadius = 1f;
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float knockbackForce = 12f;
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
    enemyAnimation = GetComponent<EnemyAnimation>();

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
        SOCKHIM();
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


  private void MeleeATK()
{
    if (playerTransform == null)
        return;

    Vector3 attackPosition = transform.position + transform.forward * 0.9f;

    Collider[] hits = Physics.OverlapSphere(
        attackPosition,
        meleeRadius,
        playerLayerMask
    );

    foreach (Collider hit in hits)
    {
        if (hit.TryGetComponent(out  Player playerCurrentHP))
        {
            playerCurrentHP.TakeDamage(meleeDamage);
        }

        if (hit.TryGetComponent(out Rigidbody playerRb))
        {
           Vector3 knockbackDirection =
        playerRb.position - transform.position;

    knockbackDirection.y = 0f;
    knockbackDirection.Normalize();

    playerRb.AddForce(
        knockbackDirection * knockbackForce,
        ForceMode.VelocityChange);
        }

    }
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
    Debug.Log("isEnemyPatrolling: " + !isPlayerVisible);
}
   private void FetchHIM()
{
    if (playerTransform != null)
    {
         isSwimming  = true;
        isAttacking = false;
         
        Vector3 direction =
            (playerTransform.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }
    enemyAnimation.animator.SetBool("isSwimming", true);
    enemyAnimation.animator.SetBool("isAttacking", false);
}
    
    private void SOCKHIM()
{
    if (isPlayerInRange)
    {

        isSwimming = false;
        isAttacking = true;
        rb.linearVelocity = Vector3.zero;

        if (playerTransform != null)
        {
            transform.LookAt(playerTransform);
        }

        if (!isOnAttackCooldown)
        {
            MeleeATK();
            StartCoroutine(AttackcooldownRoutine());
        }
    }
    enemyAnimation.animator.SetBool("isAttacking", true);
    enemyAnimation.animator.SetBool("isSwimming", false);
}


}