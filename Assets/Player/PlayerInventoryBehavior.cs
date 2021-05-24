#nullable enable

using UnityEngine;

/** Behavior to manage the player's inventory. */
public sealed class PlayerInventoryBehavior : MonoBehaviour
{
    [SerializeField] public Inventory Inventory = null!;
}
