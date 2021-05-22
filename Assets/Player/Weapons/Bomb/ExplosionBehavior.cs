using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExplosionBehavior : MonoBehaviour
{
    private void Awake()
    {
        var animator = GetComponent<Animator>();
        var animationStateMachine = animator.GetBehaviour<ExplosionAnimationStateMachineBehavior>();

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
