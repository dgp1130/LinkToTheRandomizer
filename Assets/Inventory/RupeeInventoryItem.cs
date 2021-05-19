using UnityEngine;

/** Represents some rupees held in the player's inventory. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/RupeeInventoryItem")]
public class RupeeInventoryItem : InventoryItem
{
    /** The amount of rupees to be given to the player. */
    [SerializeField] int Amount;

    public override void GiveTo(Inventory inventory)
    {
        inventory.Rupees += Amount;
    }
}
