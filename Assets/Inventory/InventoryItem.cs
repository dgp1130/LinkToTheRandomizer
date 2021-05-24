#nullable enable

using UnityEngine;

/** Base class for an item held in the player's inventory. */
public abstract class InventoryItem : ScriptableObject
{
    /** Gives the item to the given inventory. */
    public abstract void GiveTo(Inventory inventory);
}
