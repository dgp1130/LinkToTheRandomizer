#nullable enable

using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehavior))]
[RequireComponent(typeof(PlayerInventoryBehavior))]
public class PlayerBombBehavior : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab = null!;
    private PlayerMovementBehavior movementBehavior = null!;
    private PlayerInventoryBehavior inventoryBehavior = null!;

    private Inventory inventory { get => inventoryBehavior.Inventory; }

    private void Awake()
    {
        movementBehavior = GetComponent<PlayerMovementBehavior>();
        inventoryBehavior = GetComponent<PlayerInventoryBehavior>();
    }

    /** Executed when the player presses the "Place Bomb" button. */
    private void OnPlaceBomb()
    {
        var bombsItem = inventory.Bombs;
        if (!bombsItem) return; // Player has not yet picked up bombs.

        // Get the position in front of the player to spawn the bomb at.
        var bombSpawnPos = transform.position + (Vector3) getBombDropLocation();

        // Verify nothing is already in the space which would block the bomb.
        if (Physics2D.OverlapBox(bombSpawnPos, 2 * Vector2.one, 0)) return;

        // Spawn the bomb as a sibling object.
        BombBehavior.NextBombDamage = bombsItem!.Damage.Create();
        Instantiate(bombPrefab, bombSpawnPos, Quaternion.identity, transform.parent);
    }

    /** Gets location relative to this game object where the bomb will be dropped. */
    private Vector2 getBombDropLocation()
    {
        var playerExtent = movementBehavior.ForwardExtent;

        // Bomb is two units square, so its extent from the center is one unit.
        var bombExtent = 1 * movementBehavior.Direction.ToVector();

        // Add a "fudge factor" to place the bomb slightly further away than the precisely
        // calculated minimum distance, so rounding errors don't cause collisions where
        // they shouldn't.
        var fudgeFactor = movementBehavior.Direction.ToVector() * 0.25f;

        return playerExtent + bombExtent + fudgeFactor;
    }
}
