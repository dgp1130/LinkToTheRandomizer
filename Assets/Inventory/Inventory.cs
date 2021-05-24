#nullable enable

using System;
using UnityEngine;

/**
 * The player's inventory, including all items the player can hold.
 * All fields should be `NonSerialized` so they are not persisted between game runs.
 */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/Inventory")]
public sealed class Inventory : ScriptableObject
{
    /** The number of rupees held by the player. */
    [NonSerialized] public int Rupees = 0;

    /** The sword held by the player. */
    [NonSerialized] public SwordInventoryItem? Sword;

    /** The bow held by the player. */
    [NonSerialized] public BowInventoryItem? Bow;

    /** The bombs held by the player. */
    [NonSerialized] public BombInventoryItem? Bombs;
}
