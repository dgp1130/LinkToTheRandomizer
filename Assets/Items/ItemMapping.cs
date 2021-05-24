#nullable enable

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "LinkToTheRandomizer/ItemMapping")]
public sealed class ItemMapping : ScriptableObject
{
    [SerializeField] GameObject swordPrefab = null!;
    [SerializeField] GameObject bowPrefab = null!;
    [SerializeField] GameObject bombPrefab = null!;
    [SerializeField] GameObject bluepeePrefab = null!;

    public GameObject GetPrefab(Item item)
    {
        return item switch
        {
            Item.Sword => swordPrefab,
            Item.Bow => bowPrefab,
            Item.Bomb => bombPrefab,
            Item.Bluepee => bluepeePrefab,
            var val => throw new ArgumentException($"Unknown item: {val}"),
        };
    }
}
