#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public sealed class PlayerMovementBehavior : MonoBehaviour
{
    private Rigidbody2D body = null!;
    private Animator animator = null!;
    private Vector2 moveVec = Vector2.zero;
    private bool canMove = true;
    [SerializeField] float speed = 1.0f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        body.MovePosition(body.position + (moveVec * Time.deltaTime * speed));
        animate();
    }

    private void OnMove(InputValue input)
    {
        moveVec = input.Get<Vector2>();
    }

    private void animate()
    {
        if (moveVec.y > 0) animator.SetTrigger("Walk Up");
        else if (moveVec.y < 0) animator.SetTrigger("Walk Down");
        else if (moveVec.x < 0) animator.SetTrigger("Walk Left");
        else if (moveVec.x > 0) animator.SetTrigger("Walk Right");
        else stopAnimations();
    }

    // Note: Animations are played every frame, and triggers are reset after a transition
    // is taken. This means that two frames of holding left will set the "Walk Left"
    // trigger and transition from "Idle" to the "Walk Left" state. Because of this
    // transition, the "Walk Left" trigger is reset and no triggers are set at the end of
    // the first frame.  However, on the second frame, the player is still holding left
    // which sets the "Walk Left" trigger again. However, the "Walk Left" state does not
    // have a self-edge, so it does *not* consume the "Walk Left" trigger, and that trigger
    // is left in a set position. This means that any time the player stops, we need to make
    // sure to reset all the walk triggers so the existing "Walk Left" trigger does not get
    // picked up to start a new animation that doesn't match user input.
    // https://forum.unity.com/threads/animator-bug-trigger-does-not-disable-automatically-unity-5-6-0f2-64-bit.466660/#post-3040219
    private void stopAnimations()
    {
        animator.ResetTrigger("Walk Up");
        animator.ResetTrigger("Walk Down");
        animator.ResetTrigger("Walk Left");
        animator.ResetTrigger("Walk Right");
        animator.SetTrigger("Stop");
    }

    private readonly List<IEnumerator> stopCallbackQueue =
            new List<IEnumerator>();

    /**
     * <summary>
     * Stops the player's movement and enumerates the given yield instructions, waiting
     * on them, and then resuming movement when the callback completes. Movement controls
     * are disabled during this time period.
     * 
     * If there is a call to `Stop()` while it is executing a previous set of yield
     * instructions, it will wait until the first set of yield instructions finishes
     * before starting the newly provided yield instructions. Only once all requested
     * yield instructions from all `Stop()` calls have completed will the player resume
     * movement.
     * 
     * Note: When `Stop()` completes, it does **not** mean the player is now free to move.
     * It only means that particular set of yield instructions has completed and is no
     * longer holding the player. Another operation could have called `Stop()` afterwards
     * and still be stopping the player. 
     * </summary>
     * 
     * <param name="yieldInstructions">The yield instructions to execute once the player
     * has stopped and all pending yield instructions have completed. This is an
     * `IEnumerator`, meaning it is evaluated lazily and holds yield instructions for
     * Unity. Player movement will not resume until all instructions have finished.</param>
     * 
     * <example>
     * <code>
     * [RequiredComponent(typeof(PlayerMovementBehavior))]
     * public class SomeBehavior : MonoBehavior
     * {
     *     private PlayerMovementBehavior movementBehavior;
     *     
     *     private void Awake()
     *     {
     *         movementBehavior = GetComponent&lt;PlayerMovementBehavior&gt;();
     *     }
     *     
     *     private IEnumerator stopAndSlashSword()
     *     {
     *         // Stop the player and trigger the slash sword function.
     *         // Ideally, this would be an anonymous function, but unfortunately
     *         // C# does not support anonymous coroutines.
     *         yield return movementBehavior.Stop(slashSword());
     *     }
     *     
     *     private IEnumerator slashSword()
     *     {
     *         // Player is now stopped and cannot move.
     *         // Start the relevant animation and wait for it to complete.
     *         startSwordSlashAnimation();
     *         yield return new WaitUntilSwordSlashAnimationCompletes();
     *         
     *         // Animation has finished, completing this function no longer blocks
     *         // player movement.
     *     }
     * }
     * </code>
     * </example>
     */
    public IEnumerator Stop(IEnumerator yieldInstructions)
    {
        // Stop movement in case this is the first queued call to Stop().
        if (canMove)
        {
            canMove = false;
            stopAnimations();
        }

        // Create an IEnumerator which waits for the last callback in the queue and then
        // runs the given callback.
        var stopper = queue(stopCallbackQueue.LastOrDefault(), yieldInstructions);

        // Add to the queue so the next call to Stop() will properly wait for it to go
        // first.
        stopCallbackQueue.Add(stopper);

        // Wait for all previous items in the queue and this callback to complete.
        yield return stopper;
        
        // Remove the given callback from the queue now that it is completed.
        stopCallbackQueue.Remove(stopper);

        // Resume movement if all callbacks have been completed.
        if (stopCallbackQueue.Count == 0) canMove = true;
    }

    private IEnumerator queue(IEnumerator? prev, IEnumerator yieldInstructions)
    {
        if (prev != null) yield return prev;
        yield return yieldInstructions;
    }
}
