#nullable enable

using System;
using UnityEngine;

public sealed class ItemBehavior : MonoBehaviour
{
    /** The `InventoryItem` to be given to the player's inventory when picked up. */
    [SerializeField] InventoryItem? inventoryItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;
        var player = collision.gameObject;

        // Delete the item from the game world.
        Destroy(gameObject);
        
        // Get the player's inventory.
        var inventory = player.GetComponent<PlayerInventoryBehavior>().Inventory;
        if (!inventory)
        {
            throw new InvalidOperationException($"No inventory on player: {player}");
        }

        // Give the associated item to the player's inventory.
        if (inventoryItem == null)
        {
            throw new ArgumentException($"No inventory item to give to the player.");
        }
        inventoryItem.GiveTo(inventory);
    }
}
