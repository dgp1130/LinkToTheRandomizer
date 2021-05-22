#nullable enable

using System;
using UnityEngine;

public class ExplosionAnimationStateMachineBehavior : StateMachineBehaviour
{
    /** An event triggered when the explosion animation is finished. */
    public event EventHandler? AnimationFinished;

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        AnimationFinished?.Invoke(this, new EventArgs());
    }
}
