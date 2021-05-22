#nullable enable

using System;
using UnityEngine;

/**
 * <summary>
 * Represents the defense of a particular game object. An object may have different defense
 * across different kinds of attacks. For example, an enemy may be immune to ice attacks,
 * but especially vulnerable to fire attacks.
 * </summary>
 */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/Defense")]
public class Defense : ScriptableObject
{
    /** Resistence to base damage. */
    [SerializeField] public Resistence? BaseResist;
    /** Resistence to explosive damage. */
    [SerializeField] public Resistence? ExplosiveResist;

    /**
     * Applies this defense to the given damage object and returns a new,
     * reduced damage object to apply to the target of the attack.
     */
    public Damage Reduce(Damage damage)
    {
        return Damage.From(
            baseDamage: getResist(BaseResist)(damage.BaseDamage),
            explosiveDamage: getResist(ExplosiveResist)(damage.ExplosiveDamage)
        );
    }

    /** Returns the function which applies resistence to a damage value. */
    private static Func<int, int> getResist(Resistence? resistence)
    {
        // If no resistence value is present, don't resist it at all.
        static int identity(int damage) => damage;
        return resistence ? resistence!.Resist : (Func<int, int>) identity;
    }
}
