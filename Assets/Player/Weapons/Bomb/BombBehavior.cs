#nullable enable

using System;
using System.Collections;
using UnityEngine;

/** Manages a placed bomb in the world and detonates it after the appropriate length of time. */
[RequireComponent(typeof(Animator))]
public class BombBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab = null!;
    private Damage damage = null!;

    /**
     * The damage config to use for the next `BombBehavior` to be instantiated.
     * 
     * Before calling `Instantiate()` on an object with a `BombBehavior`, you *must* set
     * `BombBehavior.NextBombDamage` with the corresponding `Damage` to use for the bomb.
     */
    public static Damage? NextBombDamage { private get; set; }

    private void Awake()
    {
        // Read and validate the damage input pulled from the static reference.
        if (NextBombDamage == null) throw new ArgumentException($"{GetType().Name} requires the static {nameof(NextBombDamage)} property to be set before calling `Instantiate()`.");
        damage = NextBombDamage!;
        NextBombDamage = null; // Clear static field for next instantiation.

        var animator = GetComponent<Animator>();
        var animationStateMachine = animator.GetBehaviour<BombAnimationStateMachineBehavior>();
        StartCoroutine(detonateAfter(new WaitForEvent(
            subscribe: (cb) => animationStateMachine.PrimeAnimationFinished += cb,
            unsubscribe: (cb) => animationStateMachine.PrimeAnimationFinished -= cb
        )));
    }

    private IEnumerator detonateAfter(CustomYieldInstruction delay)
    {
        yield return delay;

        // Spawn an explosion at the same location on the parent.
        ExplosionBehavior.NextExplosionDamage = damage;
        Instantiate(
            explosionPrefab /* prefab to instantiate */,
            transform.position /* position (relative to new parent) */,
            Quaternion.identity /* rotation */,
            transform.parent /* parent of new object */
        );

        // Delete this bomb.
        Destroy(gameObject);
    }
}
