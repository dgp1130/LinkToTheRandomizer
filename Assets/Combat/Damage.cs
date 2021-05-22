/** Simple data class representing the damage done by a particular attack. */
public class Damage
{
    /** Base damage done by this attack with no special modifiers. */
    public readonly int BaseDamage;
    /** Explosive damage done by this attack. */
    public readonly int ExplosiveDamage;

    /** Total damage done across all modifiers combined. */
    public int TotalDamage { get => BaseDamage + ExplosiveDamage; }

    private Damage(int baseDamage, int explosiveDamage)
    {
        BaseDamage = baseDamage;
        ExplosiveDamage = explosiveDamage;
    }

    /** Returns a `Damage` object from the given parameters. */
    public static Damage From(int baseDamage, int explosiveDamage)
    {
        return new Damage(
            baseDamage: baseDamage,
            explosiveDamage: explosiveDamage
        );
    }
}
