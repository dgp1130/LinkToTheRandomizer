#nullable enable

using UnityEngine;

/** Behavior representing the health mechanic of an actor. */
public sealed class HealthBehavior : MonoBehaviour
{
    /** Total health for the actor. */
    [SerializeField] private int health;

    /** Receives damage and reduces health accordingly, possibly destroying the actor. */
    public void Receive(Damage damage)
    {
        health -= damage.TotalDamage;
        if (health <= 0) Destroy(gameObject);
    }
}
