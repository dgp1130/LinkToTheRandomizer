using UnityEngine;

/** Scriptable object which defines the parameters necessary to create `Attack `objects. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/Damage")]
public class DamageConfig : ScriptableObject
{
    /** The height the attack is at. */
    [SerializeField] private Height height;
    /** Base damage to deal for the associated attack. */
    [SerializeField] private int BaseDamage;
    /** Explosive damage done by this attack. */
    [SerializeField] private int ExplosiveDamage;

    /** Creates a `Damage` object with the preconfigured values. */
    public Damage Create()
    {
        return Damage.From(
            height: height,
            baseDamage: BaseDamage,
            explosiveDamage: ExplosiveDamage
        );
    }
}
