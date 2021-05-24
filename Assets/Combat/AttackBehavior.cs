#nullable enable

using System;
using UnityEngine;

/**
 * <summary>
 * Behavior which allows an object to deal damage to another object.
 * 
 * Any object with this behavior included will deal damage to any object
 * configured to receive it when they collide. Either a `DamageConfig` or
 * a `Damage` object must be provided.
 * </summary>
 */
public sealed class AttackBehavior : MonoBehaviour
{
    [SerializeField] public DamageConfig? DamageConfig;
    [NonSerialized] public Damage? DamageInput;

    private Damage? damage;
    /** The damage done by this attack. */
    public Damage Damage
    {
        get
        {
            if (damage != null) return damage; // Use cached value if present.

            // Validate inputs, we expect one xor the other.
            if (DamageInput == null && !DamageConfig) throw new ArgumentException($"{GetType().Name} requires {nameof(DamageConfig)} xor {nameof(DamageInput)} to be set, but neither were set.");
            if (DamageInput != null && DamageConfig) throw new ArgumentException($"{GetType().Name} requires {nameof(DamageConfig)} xor {nameof(DamageInput)} to be set, but *both* were set.");

            // Use whichever value we have.
            return damage ??= DamageInput ?? DamageConfig!.Create();
        }
    }

    /**
     * Triggers whenever a collision with another object (that can receive damage) is detected.
     * The `GameObject` included in the other object which was hit.
     */
    public event EventHandler<GameObject>? Hit;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Ignore any collisions that are not allowed to be hit.
        var defense = collider.gameObject.GetComponent<DefenseBehavior>();
        if (!defense) return;

        // Check if the defender successfully evaded the attack.
        if (defense.Evade(Damage)) return;

        // Successfully hit the target, apply damage.
        defense.Receive(Damage);

        Hit?.Invoke(this, collider.gameObject);
    }
}
