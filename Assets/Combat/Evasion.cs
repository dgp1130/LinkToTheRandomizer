#nullable enable

using UnityEngine;

/** Represents a target's ability to evade an incoming attack. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/Evasion")]
public sealed class Evasion : ScriptableObject
{
    /** The height of the defender as relates to incoming attacks. */
    [SerializeField] public Height Height;

    /** Returns whether or not the target evaded the damage given. */
    public bool Evade(Damage damage)
    {
        // Check if this is a short object and the attack went over it.
        if (Height == Height.Short && damage.Height == Height.Tall) return true;

        return false;
    }
}