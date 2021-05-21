#nullable enable

using UnityEngine;

/**
 * <summary>
 * Behavior which allows an object to deal damage to another object.
 * 
 * Any object with this behavior included will deal damage to any object
 * configured to receive it when they collide.
 * </summary>
 */
public class AttackBehavior : MonoBehaviour
{
    [SerializeField] private DamageConfig damageConfig = null!;

    private Damage? damage;
    /** The damage done by this attack. */
    public Damage Damage { get => damage ??= damageConfig.Create(); }
}
