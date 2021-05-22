using System.Collections;
using UnityEngine;

/** Manages a placed bomb in the world and detonates it after the appropriate length of time. */
[RequireComponent(typeof(Animator))]
public class BombBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
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
