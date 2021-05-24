#nullable enable

using UnityEngine;

/**
 * <summary>
 * Abstract data type to define the kind of resistence an actor has to a particular damage
 * type.
 * </summary>
 */
public abstract class Resistence : ScriptableObject {
    public abstract int Resist(int damage);
}
