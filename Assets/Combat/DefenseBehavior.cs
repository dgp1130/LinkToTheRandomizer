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
    /** The evasion stat to apply to all incoming attacks. */
    [SerializeField] public Evasion? Evasion;
    /** The defense stat to apply to all incoming attacks. */
    [SerializeField] public Defense? Defense;
    /** The target of any received attacks. Must have a `HealthBehavior` component. */
    [Tooltip("The target being defended. Must have a `HealthBehavior` component.")]
    [SerializeField] private GameObject target = null!;
    /** Whether or not this defense guards a target which has a `HealthBehavior. */
    [Tooltip("Whether or not this defense is guarding a target with health. `target` must be set when this is true.")]
    [SerializeField] private bool hasHealth = true;
    private HealthBehavior? healthBehavior;

    private void Awake()
    {
        if (hasHealth)
        {
            healthBehavior = target.GetComponent<HealthBehavior>();
            if (!healthBehavior)
            {
                throw new InvalidOperationException(
                    $"DefenseBehavior requires that the target object ({target}) has a HealthBehavior, but it does not.");
            }
        }
    }

    /** Returns whether or not the defender successfully evaded the incoming attack. */
    public bool Evade(Damage damage)
    {
        return Evasion ? Evasion!.Evade(damage) : false;
    }

    /** Receives some damage from an attack. */
    public void Receive(Damage damage)
    {
        if (healthBehavior)
        {
            // Apply the defense stat to reduce damage.
            var reduced = Defense ? Defense!.Reduce(damage) : damage;

            // Apply resulting damage to the target.
            healthBehavior!.Receive(reduced);
        }

        Hit?.Invoke(this, new EventArgs());
    }

    /** Triggered when the object is hit by an attack. */
    public event EventHandler? Hit;
}
