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
    /** Resistance to base damage. */
    [SerializeField] public Resistence BaseResist;

    /**
     * Applies this defense to the given damage object and returns a new,
     * reduced damage object to apply to the target of the attack.
     */
    public Damage Reduce(Damage damage)
    {
        return Damage.From(
            baseDamage: BaseResist.Resist(damage.BaseDamage)
        );
    }
}
