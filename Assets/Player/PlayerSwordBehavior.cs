#nullable enable

using System.Collections;
using UnityEngine;

/** Behavior controlling the player's sword slashing functionality. */
[RequireComponent(typeof(PlayerInventoryBehavior))]
[RequireComponent(typeof(PlayerMovementBehavior))]
[RequireComponent(typeof(Animator))]
public sealed class PlayerSwordBehavior : MonoBehaviour
{
    private PlayerInventoryBehavior inventoryBehavior = null!;
    private Inventory inventory { get => inventoryBehavior.Inventory; }
    private PlayerMovementBehavior movementBehavior = null!;
    private Animator animator = null!;
    private PlayerAnimationStateMachineBehavior animatorStateMachine = null!;

    private void Awake()
    {
        inventoryBehavior = GetComponent<PlayerInventoryBehavior>();
        movementBehavior = GetComponent<PlayerMovementBehavior>();
        animator = GetComponent<Animator>();
        animatorStateMachine = animator.GetBehaviour<PlayerAnimationStateMachineBehavior>();
    }

    /** Executed when the player presses the "Slash Sword" button. */
    private IEnumerator OnSlashSword()
    {
        // Don't slash if player doesn't have a sword.
        if (!inventory.Sword) yield break;

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
