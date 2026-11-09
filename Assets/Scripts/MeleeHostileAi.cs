using System.Collections;
using UnityEngine;

public class MeleeHostileAi : MonoBehaviour
{
    [SerializeField] private CapsuleCollider meleeCollider;
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject MeleeSiren;

    [SerializeField] private LayerMask Terrain;
    [SerializeField] private LayerMask playerLayerMask;

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
    [SerializeField] private float visionRange = 40f;

    [Header("Move and Attack")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float meleeRadius = 1f;
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private EnemyAnimation enemyAnimation;

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

    rb = GetComponent<Rigidbody>();
    enemyAnimation = GetComponent<EnemyAnimation>();

    if (meleeCollider == null && MeleeSiren != null)
    {
        meleeCollider = MeleeSiren.GetComponent<CapsuleCollider>();
    }
} 

    private void Update()
    {
        DetectPlayer();
        UpdateBehaviourState();
    }

    // =========================
    // DETECTION
    // =========================

  private void DetectPlayer()
{
    isPlayerVisible = Physics.CheckSphere(
        transform.position,
        visionRange,
        playerLayerMask
    );

    if (meleeCollider != null && playerTransform != null)
    {
        float distance = Vector3.Distance(
            meleeCollider.bounds.center,
            playerTransform.position
        );

        isPlayerInRange = distance <= attackDistance;
    }
}

    // =========================
    // BEHAVIOUR
    // =========================

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

    // =========================
    // ANIMATIONS
    // =========================

    private void UpdateAnimations(bool swimming, bool attacking)
    {
        isSwimming = swimming;
        isAttacking = attacking;

        if (animator != null)
        {
            animator.SetBool("isSwimming", swimming);
            animator.SetBool("isAttacking", attacking);
        }
    }

    // =========================
    // LOOK / ROTATION
    // =========================

    private void LookAtPlayer()
    {
        if (playerTransform == null)
            return;

        Vector3 direction = playerTransform.position - transform.position;

        // Ignore vertical differences.
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }

    private void LookAtMovementDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }

    // =========================
    // PATROL
    // =========================

    private void FindPatrolPoint()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);

        // Keep patrol points at the enemy's current height.
        currentPatrolPoint = transform.position + new Vector3(
            randomX,
            0f,
            randomZ
        );

        hasPatrolPoint = true;
    }

    private void FindPatrolPath()
    {
        UpdateAnimations(false, false);

        if (!hasPatrolPoint)
            FindPatrolPoint();

        Vector3 direction = currentPatrolPoint - transform.position;

        // Ignore vertical movement.
        direction.y = 0f;
        direction.Normalize();

        if (Vector3.Distance(
            new Vector3(transform.position.x, 0f, transform.position.z),
            new Vector3(currentPatrolPoint.x, 0f, currentPatrolPoint.z)
        ) < 1f)
        {
            hasPatrolPoint = false;

            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f
            );

            return;
        }

        MoveHorizontally(direction);

        LookAtMovementDirection(direction);
    }

    // =========================
    // CHASE PLAYER
    // =========================

    [Header("Chase Settings")]
[SerializeField] private Vector3 playerOffset = new Vector3(0f, -2f, 0f);
[SerializeField] private float attackDistance = 2.5f;



private void FetchHIM()
{
    if (playerTransform == null)
        return;

    UpdateAnimations(true, false);

    // Calculate an offset relative to the player's rotation.
    Vector3 targetPosition =
        playerTransform.TransformPoint(playerOffset);

    Vector3 direction = targetPosition - transform.position;

    direction.Normalize();

    rb.linearVelocity = direction * moveSpeed;

    LookAtMovementDirection(direction);
}

    // =========================
    // MELEE ATTACK
    // =========================

    private void SOCKHIM()
{
    UpdateAnimations(false, true);

    // Completely stop the enemy's movement.
    rb.linearVelocity = Vector3.zero;

    LookAtPlayer();

    if (!isOnAttackCooldown)
    {
        MeleeATK();
        StartCoroutine(AttackcooldownRoutine());
    }
}

    private void MeleeATK()
    {
        if (playerTransform == null)
            return;

    Vector3 attackPosition = meleeCollider.bounds.center + transform.forward * 0.9f;
        Collider[] hits = Physics.OverlapSphere(
            attackPosition,
            meleeRadius,
            playerLayerMask
        );

        foreach (Collider hit in hits)
        {
            Player player = hit.GetComponentInParent<Player>();

            if (player != null)
            {
                player.TakeDamage(meleeDamage);
            }

            Rigidbody playerRb = hit.GetComponentInParent<Rigidbody>();

            if (playerRb != null)
            {
                Vector3 knockbackDirection =
                    playerRb.position - transform.position;

                knockbackDirection.y = 0f;
                knockbackDirection.Normalize();

                playerRb.AddForce(
                    knockbackDirection * knockbackForce,
                    ForceMode.VelocityChange
                );
            }
        }
    }

    // =========================
    // MOVEMENT
    // =========================

    private void MoveHorizontally(Vector3 direction)
    {
        // Preserve existing vertical velocity.
        // Gravity and other vertical physics remain unaffected.
        rb.linearVelocity = new Vector3(
            direction.x * moveSpeed,
            rb.linearVelocity.y,
            direction.z * moveSpeed
        );
    }

    // =========================
    // ATTACK COOLDOWN
    // =========================

    private IEnumerator AttackcooldownRoutine()
    {
        isOnAttackCooldown = true;

        yield return new WaitForSeconds(AttackCooldown);

        isOnAttackCooldown = false;
    }

    // =========================
    // GIZMOS
    // =========================

    private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;

    Gizmos.DrawWireSphere(
        transform.position,
        visionRange
    );

    Gizmos.color = Color.yellow;

    Vector3 center = meleeCollider != null
        ? meleeCollider.bounds.center
        : transform.position;

    Gizmos.DrawWireSphere(
        center,
        attackDistance
    );
}
}
