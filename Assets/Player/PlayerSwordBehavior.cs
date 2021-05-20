using System.Collections;
using UnityEngine;

/** Behavior controlling the player's sword slashing functionality. */
[RequireComponent(typeof(PlayerMovementBehavior))]
[RequireComponent(typeof(Animator))]
public sealed class PlayerSwordBehavior : MonoBehaviour
{
    private PlayerMovementBehavior movementBehavior;
    private Animator animator;
    private PlayerAnimationStateMachineBehavior animatorStateMachine;

    private void Awake()
    {
        movementBehavior = GetComponent<PlayerMovementBehavior>();
        animator = GetComponent<Animator>();
        animatorStateMachine = animator.GetBehaviour<PlayerAnimationStateMachineBehavior>();
    }

    /** Executed when the player presses the "Slash Sword" button. */
    private IEnumerator OnSlashSword()
    {
        yield return movementBehavior.Stop(slashSword());
    }

    /**
     * Slashes the sword by playing the relevant animation and waiting for that
     * animation to complete.
     */
    private IEnumerator slashSword()
    {
        yield return new WaitForEvent(
            subscribe: (cb) => animatorStateMachine.SwordSlashFinished += cb,
            unsubscribe: (cb) => animatorStateMachine.SwordSlashFinished -= cb,
            start: () => animator.SetTrigger("Slash Sword")
        );
    }
}
