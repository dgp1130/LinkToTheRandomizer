#nullable enable

using System;
using UnityEngine;

public class BombAnimationStateMachineBehavior : StateMachineBehaviour
{
    /** An event triggered when the bomb priming animation is finished. */
    public event EventHandler? PrimeAnimationFinished;

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        PrimeAnimationFinished?.Invoke(this, new EventArgs());
    }
}
