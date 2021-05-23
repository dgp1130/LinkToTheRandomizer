#nullable enable

using UnityEngine;

/** Manages the game logic for a bridge. */
public sealed class BridgeBehavior : MonoBehaviour
{
    [Tooltip("Whether or not the bridge is visible for actors to cross with.")]
    [SerializeField] private bool crossable = true;

    /**
     * Whether or not actors can cross the bridge. When `false`, the bridge is hidden
     * and has a collider which prevents movement over that space (make sure to put a
     * water tile underneath this!). When `true`, the bridge is visible and allows
     * actors to move through it.
     */
    public bool Crossable
    {
        get => crossable;
        set
        {
            crossable = value;
            onCrossableChanged();
        }
    }

    private SpriteRenderer sprite = null!;
    private new BoxCollider2D collider = null!;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        onCrossableChanged();
    }

    private void onCrossableChanged()
    {
        if (crossable)
        {
            // Player can cross the bridge, show it and disable the collider.
            sprite.enabled = true;
            collider.enabled = false;
        }
        else
        {
            // Player *cannot* cross the bridge, hide it and enable the collider.
            sprite.enabled = false;
            collider.enabled = true;
        }
    }
}
