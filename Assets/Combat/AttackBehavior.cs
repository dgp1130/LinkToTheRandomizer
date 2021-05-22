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
public class AttackBehavior : MonoBehaviour
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
            if (DamageInput == null && !DamageConfig) throw new ArgumentException($"{GetType().Name} requires {nameof(DamageConfig)} xor {nameof(DamageInput)} to be set, but neither set set.");
            if (DamageInput != null && DamageConfig) throw new ArgumentException($"{GetType().Name} requires {nameof(DamageConfig)} xor {nameof(DamageInput)} to be set, but *both* were set.");

            // Use whichever value we have.
            return damage ??= DamageInput ?? DamageConfig!.Create();
        }
    }
}
