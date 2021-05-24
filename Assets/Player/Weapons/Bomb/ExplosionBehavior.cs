#nullable enable

using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public sealed class ExplosionBehavior : MonoBehaviour
{
    [SerializeField] private GameObject hitBox = null!;

    /**
     * The damage config to use for the next `ExplosionBehavior` to be instantiated.
     * 
     * Before calling `Instantiate()` on an object with a `ExplosionBehavior`, you *must* set
     * `ExplosionBehavior.NextExplosionDamage` with the corresponding `Damage` to use for the
     * bomb.
     */
    public static Damage? NextExplosionDamage { private get; set; }

    private void Awake()
    {
        // Read and validate the damage input pulled from the static reference.
        if (NextExplosionDamage == null) throw new ArgumentException($"{GetType().Name} requires the static {nameof(NextExplosionDamage)} property to be set before calling `Instantiate()`.");
        var attackBehavior = hitBox.GetComponent<AttackBehavior>();
        attackBehavior.DamageInput = NextExplosionDamage!;
        NextExplosionDamage = null; // Clear static field for next instantiation.

        var animator = GetComponent<Animator>();
        var animationStateMachine =
            animator.GetBehaviour<ExplosionAnimationStateMachineBehavior>();
        StartCoroutine(dissipateAfter(new WaitForEvent(
            subscribe: (cb) => animationStateMachine.AnimationFinished += cb,
            unsubscribe: (cb) => animationStateMachine.AnimationFinished -= cb
        )));
    }

    private IEnumerator dissipateAfter(CustomYieldInstruction delay)
    {
        yield return delay;
        Destroy(gameObject);
    }
}
