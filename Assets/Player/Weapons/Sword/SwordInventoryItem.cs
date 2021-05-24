#nullable enable

using UnityEngine;

/** Represents a sword held in the player's inventory. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/SwordInventoryItem")]
public sealed class SwordInventoryItem : InventoryItem
{
    [SerializeField] int Damage;

    public override void GiveTo(Inventory inventory)
    {
        inventory.Sword = this;
    }
}
