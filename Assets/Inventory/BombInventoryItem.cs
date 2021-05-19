using UnityEngine;

/** Represents bombs held in the player's inventory. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/BombInventoryItem")]
public class BombInventoryItem : InventoryItem
{
    /** How much damage a single bomb explosion does. */
    [SerializeField] int Damage;

    /** How many bombs a player can hold in total. */
    [SerializeField] int MaxBombs;

    public override void GiveTo(Inventory inventory)
    {
        inventory.Bombs = this;
    }
}
