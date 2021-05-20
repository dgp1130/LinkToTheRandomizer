#nullable enable

using System;
using UnityEngine;

/** Yield instruction for Unity to wait until an event is triggered once. */
public sealed class WaitForEvent : CustomYieldInstruction
{
    private bool eventTriggered = false;

    /**
     * <summary>Creates a yield instruction which waits until an event is triggered
     * once.</summary>
     * 
     * <param name="subscribe">A callback which subscribes the given event handler
     * to the relevant event.</param>
     * <param name="unsubscribe">A callback which unsubscribes the given event
     * handler from the relevant event.</param>
     * <param name="start">A callback invoked after the event is subscribed to.
     * Operations performed here are guaranteed to be picked up by `WaitForEvent`,
     * even if it immediately triggers the associated event.</param>
     * 
     * <example>
     * <code>
     * private event EventHandler onFoo;
     * // ...
     * new WaitForEvent(
     *     subscribe: (cb) => onFoo += cb,
     *     unsubscribe: (cb) => onFoo -= cb,
     *     start: () => doSomethingThatTriggersOnFoo()
     * );
     * </code>
     * </example>
     */
    public WaitForEvent(
        Action<EventHandler> subscribe,
        Action<EventHandler> unsubscribe,
        Action? start = null)
    {
        void onEventTriggered(object sender, EventArgs args)
        {
            eventTriggered = true;
            unsubscribe(onEventTriggered);
        }

        subscribe(onEventTriggered);
        start?.Invoke();
    }

    public override bool keepWaiting => !eventTriggered;
}
