#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    private ImmutableDictionary<Direction, GameObject> hitBoxes = null!;

    private void Awake()
    {
        inventoryBehavior = GetComponent<PlayerInventoryBehavior>();
        movementBehavior = GetComponent<PlayerMovementBehavior>();
        animator = GetComponent<Animator>();
        animatorStateMachine = animator.GetBehaviour<PlayerAnimationStateMachineBehavior>();

        // Load hitboxes and map to the direction they swing.
        hitBoxes = hitBoxNames.Select((pair) => new KeyValuePair<Direction, GameObject>(
            pair.Key,
            transform.Find(pair.Value).gameObject
        )).ToImmutableDictionary();

        // Disable all hitboxes at the start.
        foreach (var hitBox in hitBoxes.Values) hitBox.SetActive(false);
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
        // Get the hitbox for the direction the player is currently facing.
        var hitBox = hitBoxes[movementBehavior.Direction];

        // Enable the hitbox so it will strike other objects.
        hitBox.SetActive(true);

        // Wait for the swing animation to complete.
        yield return new WaitForEvent(
            subscribe: (cb) => animatorStateMachine.SwordSlashFinished += cb,
            unsubscribe: (cb) => animatorStateMachine.SwordSlashFinished -= cb,
            start: () => animator.SetTrigger("Slash Sword")
        );

        // Disable the hitbox as the player is no longer swinging.
        hitBox.SetActive(false);
    }

    /** Maps all directions to their associated sword hit box game object's name. */
    private static readonly ImmutableDictionary<Direction, string> hitBoxNames =
        new Dictionary<Direction, string>
    {
        { Direction.North, "Slash Up HitBox" },
        { Direction.South, "Slash Down HitBox" },
        { Direction.West, "Slash Left HitBox" },
        { Direction.East, "Slash Right HitBox" },
    }.ToImmutableDictionary();
}
