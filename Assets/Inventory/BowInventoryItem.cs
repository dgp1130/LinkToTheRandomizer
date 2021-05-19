using UnityEngine;

/** Represents a bow held in the player's inventory. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/BowInventoryItem")]
public class BowInventoryItem : InventoryItem
{
    /** How much damage a single arrow does. */
    [SerializeField] int Damage;

    /** How many arrows a player can hold in total. */
    [SerializeField] int MaxArrows;

    public override void GiveTo(Inventory inventory)
    {
        inventory.Bow = this;
    }
}
