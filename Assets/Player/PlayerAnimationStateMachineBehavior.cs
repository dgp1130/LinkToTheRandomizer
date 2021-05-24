#nullable enable

using System;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

/** State machine behavior for player animations. */
public sealed class PlayerAnimationStateMachineBehavior : StateMachineBehaviour
{
    /** An event triggered when a sword slash animation is finished. */
    public event EventHandler? SwordSlashFinished;

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    ) {
        if (isSlashSwordState(stateInfo)) SwordSlashFinished?.Invoke(this, new EventArgs());
    }

    /** Returns whether or not a given state is a sword slash state. */
    private bool isSlashSwordState(AnimatorStateInfo stateInfo)
    {
        return slashSwordStateNames.Any((name) => stateInfo.IsName(name));
    }

    private static readonly ImmutableList<string> slashSwordStateNames = ImmutableList.Create(
        "Slash Sword Up",
        "Slash Sword Down",
        "Slash Sword Left",
        "Slash Sword Right"
    );
}
