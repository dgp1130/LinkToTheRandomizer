#nullable enable

/** Simple data class representing the damage done by a particular attack. */
public sealed class Damage
{
    /** The height the attack is at. */
    public readonly Height Height;
    /** Base damage done by this attack with no special modifiers. */
    public readonly int BaseDamage;
    /** Explosive damage done by this attack. */
    public readonly int ExplosiveDamage;

    /** Total damage done across all modifiers combined. */
    public int TotalDamage { get => BaseDamage + ExplosiveDamage; }

    private Damage(Height height, int baseDamage, int explosiveDamage)
    {
        Height = height;
        BaseDamage = baseDamage;
        ExplosiveDamage = explosiveDamage;
    }

    /** Returns a `Damage` object from the given parameters. */
    public static Damage From(Height height, int baseDamage, int explosiveDamage)
    {
        return new Damage(
            height: height,
            baseDamage: baseDamage,
            explosiveDamage: explosiveDamage
        );
    }
}
