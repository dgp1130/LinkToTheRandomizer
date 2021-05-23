using UnityEngine;

/** Represents a bow held in the player's inventory. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/BowInventoryItem")]
public class BowInventoryItem : InventoryItem
{
    /** How much damage a single arrow does. */
    [SerializeField] public DamageConfig Damage;

    /** Speed of the arrow in units / second. */
    [Tooltip("Speed of an arrow in units per second.")]
    [SerializeField] public float Speed;

    /** How many arrows a player can hold in total. */
    [SerializeField] public int MaxArrows;

    public override void GiveTo(Inventory inventory)
    {
        inventory.Bow = this;
    }
}
