using UnityEngine;

/** Scriptable object which defines the parameters necessary to create `Attack `objects. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/Damage")]
public class DamageConfig : ScriptableObject
{
    /** Base damage to deal for the associated attack. */
    [SerializeField] private int BaseDamage;
    /** Explosive damage done by this attack. */
    [SerializeField] private int ExplosiveDamage;

    /** Creates a `Damage` object with the preconfigured values. */
    public Damage Create()
    {
        return Damage.From(
            baseDamage: BaseDamage,
            explosiveDamage: ExplosiveDamage
        );
    }
}
