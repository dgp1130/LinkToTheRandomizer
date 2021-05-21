/** Simple data class representing the damage done by a particular attack. */
public class Damage
{
    /** Base damage done by this attack with no special modifiers. */
    public readonly int BaseDamage;

    /** Total damage done across all modifiers combined. */
    public int TotalDamage { get => BaseDamage; }

    private Damage(int baseDamage)
    {
        BaseDamage = baseDamage;
    }

    /** Returns a `Damage` object from the given parameters. */
    public static Damage From(int baseDamage)
    {
        return new Damage(
            baseDamage: baseDamage
        );
    }
}
