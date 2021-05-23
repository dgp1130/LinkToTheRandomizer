#nullable enable

using System;
using UnityEngine;

/** Manages a fired arrow in the world. */
[RequireComponent(typeof(Renderer))]
public sealed class ArrowBehavior : MonoBehaviour
{
    [SerializeField] private GameObject hitBox = null!;
    private new Renderer renderer = null!;

    /** Speed of the arrow in units per second. */
    private float speed;

    /**
     * Parameters to use for the next `ArrowBehavior` to be instantiated.
     * 
     * Before calling `Instantiate()` on an object with a `ArrowBehavior`, you *must* set
     * `ArrowBehavior.NextArrowParams` with the corresponding parameters to use for the
     * arrow.
     */
    public static Params? NextArrowParams;

    private void Awake()
    {
        // Read and validate the damage input pulled from the static reference.
        if (NextArrowParams == null) throw new ArgumentException($"{GetType().Name} requires the static {nameof(NextArrowParams)} property to be set before calling `Instantiate()`.");
        var @params = NextArrowParams!;
        NextArrowParams = null; // Clear static field for next instantiation.

        var attackBehavior = hitBox.GetComponent<AttackBehavior>();
        attackBehavior.DamageInput = @params.Damage;
        attackBehavior.Hit += onHit;

        speed = @params.Speed;

        renderer = GetComponent<Renderer>();
    }

    private void OnDestroy()
    {
        var attackBehavior = hitBox.GetComponent<AttackBehavior>();
        attackBehavior.Hit -= onHit;
    }

    private void FixedUpdate()
    {
        // Delete the arrow once it flies offscreen.
        if (!renderer.isVisible) Destroy(gameObject);

        // Move the arrow forward each frame.
        var forward = transform.rotation * Vector3.up;
        transform.position += forward * speed * Time.deltaTime;
    }

    // Destory the arrow when it hits anything.
    private void onHit(object sender, GameObject target) => Destroy(gameObject);

    /** Parameters for a dynamically instantiated `ArrowBehavior` object. */
    public sealed class Params
    {
        /** The damage the arrow will do on hit. */
        public readonly Damage Damage;

        /** The speed the arrow moves in units per second. */
        public readonly float Speed;

        public Params(Damage damage, float speed)
        {
            Damage = damage;
            Speed = speed;
        }
    }
}
