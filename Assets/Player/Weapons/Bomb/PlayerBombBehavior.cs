#nullable enable

using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehavior))]
[RequireComponent(typeof(PlayerInventoryBehavior))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerBombBehavior : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab = null!;
    private PlayerMovementBehavior movementBehavior = null!;
    private PlayerInventoryBehavior inventoryBehavior = null!;
    private new BoxCollider2D collider = null!;

    private Inventory inventory { get => inventoryBehavior.Inventory; }

    private void Awake()
    {
        movementBehavior = GetComponent<PlayerMovementBehavior>();
        inventoryBehavior = GetComponent<PlayerInventoryBehavior>();
        collider = GetComponent<BoxCollider2D>();
    }

    /** Executed when the player presses the "Place Bomb" button. */
    private void OnPlaceBomb()
    {
        var bombsItem = inventory.Bombs;
        if (!bombsItem) return; // Player has not yet picked up bombs.

        // Get the position in front of the player to spawn the bomb at.
        var bombSpawnPos =
            transform.position + getBombDropLocation(movementBehavior.Direction);

        // Verify nothing is already in the space which would block the bomb.
        if (Physics2D.OverlapBox(bombSpawnPos, 2 * Vector2.one, 0)) return;

        // Spawn the bomb as a sibling object.
        BombBehavior.NextBombDamage = bombsItem!.Damage.Create();
        Instantiate(bombPrefab, bombSpawnPos, Quaternion.identity, transform.parent);
    }

    /** Gets location relative to this game object where the bomb will be dropped. */
    private Vector3 getBombDropLocation(Direction direction)
    {
        // Bomb is two units square, so its extent from the center is one unit.
        Vector3 bombExtent = 1 * movementBehavior.Direction.ToVector();
        Vector3 playerExtent = getExtent(direction);

        // Add a "fudge factor" to place the bomb slightly further away that the precisely
        // calculated minimum distance, so rounding errors don't cause collisions where
        // they shouldn't.
        Vector3 fudgeFactor = movementBehavior.Direction.ToVector() * 0.25f;

        return playerExtent + bombExtent + fudgeFactor;
    }

    /**
     * Gets the "extent" of the player in a particular direction. "Extent" in this context
     * aligns with the definition in the `Bounds` class, meaning the distance from the
     * center of a collider to its edge in a particular dimension.
     */
    private Vector3 getExtent(Direction direction)
    {
        var centerRelative = collider.bounds.center - transform.position;
        switch (direction)
        {
            case Direction.North:
            case Direction.South:
                return centerRelative + (Vector3) (direction.ToVector() * collider.bounds.extents.y);
            case Direction.West:
            case Direction.East:
                return centerRelative + (Vector3) (direction.ToVector() * collider.bounds.extents.x);
            default:
                throw new ArgumentException($"Unknown direction: {direction}.");
        }
    }
}
