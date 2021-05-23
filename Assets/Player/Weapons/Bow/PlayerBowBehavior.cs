#nullable enable

using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehavior))]
[RequireComponent(typeof(PlayerInventoryBehavior))]
public sealed class PlayerBowBehavior : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab = null!;
    private PlayerMovementBehavior movementBehavior = null!;
    private PlayerInventoryBehavior inventoryBehavior = null!;
    private Inventory inventory { get => inventoryBehavior.Inventory; }

    private void Awake()
    {
        movementBehavior = GetComponent<PlayerMovementBehavior>();
        inventoryBehavior = GetComponent<PlayerInventoryBehavior>();
    }

    /** Executed when the player presses the "Shoot Arrow" button. */
    private void OnShootArrow()
    {
        var bowItem = inventory.Bow;
        if (!bowItem) return; // No bow to shoot an arrow with.

        // Spawn the arrow as a sibling object in front of the player.
        var arrowSpawnPos = transform.position + (Vector3) getArrowSpawnLocation();
        var rotation = Quaternion.LookRotation(Vector3.forward, movementBehavior.Direction.ToVector());
        ArrowBehavior.NextArrowParams = new ArrowBehavior.Params(
            damage: bowItem!.Damage.Create(),
            speed: bowItem!.Speed
        );
        Instantiate(arrowPrefab, arrowSpawnPos, rotation, transform.parent);
    }

    /** Gets location relative to this game object where the arrow will be spawned. */
    private Vector2 getArrowSpawnLocation()
    {
        var playerExtent = movementBehavior.ForwardExtent;

        // Arrow is 2 units long, so its extent from the center is one unit.
        var arrowExtent = 1 * movementBehavior.Direction.ToVector();

        // Add a "fudge factor" to spawn the arrow slightly further away than the precisely
        // calculated minimum distance, so rounding errors don't cause collisions where
        // they shouldn't.
        var fudgeFactor = movementBehavior.Direction.ToVector() * 0.25f;

        return playerExtent + arrowExtent + fudgeFactor;
    }
}
