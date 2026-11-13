
using UnityEngine;

public class OrbBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform orbSpawnPoint;

    private bool animationFinished;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animationFinished || animator == null)
            return;

        AnimatorStateInfo state =
            animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the animation has finished playing.
        if (state.normalizedTime >= 1f &&
            !animator.IsInTransition(0))
        {
            FinishOrbAnimation();
        }
    }

    private void FinishOrbAnimation()
    {
        animationFinished = true;

        // Stop the animation.
        animator.enabled = false;

        // Move the orb to its designated position.
        if (orbSpawnPoint != null)
        {
            transform.position = orbSpawnPoint.position;
            transform.rotation = orbSpawnPoint.rotation;
        }
    }
}
