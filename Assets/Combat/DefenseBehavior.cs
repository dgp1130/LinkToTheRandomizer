#nullable enable

using System;
using UnityEngine;

/**
 * <summary>
 * Behavior for an object which can be attacked. Checks if any given collision is an attack
 * (has an `AttackBehavior` component), reduces its damage via the defense stat, and then
 * applies the result to the given target's `HealthBehavior`.
 * </summary>
 */
public class DefenseBehavior : MonoBehaviour
{
    /** The defense stat to apply to all incoming attacks. */
    [SerializeField] public Defense Defense = null!;
    /** The target of any received attacks. Must have a `HealthBehavior` component. */
    [SerializeField] private GameObject target = null!;
    private HealthBehavior healthBehavior = null!;

    private void Awake()
    {
        healthBehavior = target.GetComponent<HealthBehavior>();
        if (!healthBehavior)
        {
            throw new InvalidOperationException(
                $"DefenseBehavior requires that the target object ({target}) has a HealthBehavior, but it does not.");
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Ignore any collisions that are not attacks.
        var attack = collider.gameObject.GetComponent<AttackBehavior>();
        if (!attack) return;

        // Apply the defense stat to reduce damage.
        var damage = Defense.Reduce(attack.Damage);

        // Apply resulting damage to the target.
        healthBehavior.Receive(damage);
    }
}
