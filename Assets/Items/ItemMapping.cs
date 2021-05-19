using System;
using DevelWithoutACause.Randomizer;
using UnityEngine;

[CreateAssetMenu(menuName = "LinkToTheRandomizer/ItemMapping")]
public class ItemMapping : ScriptableObject
{
    [SerializeField] GameObject swordPrefab;
    [SerializeField] GameObject bowPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject bluepeePrefab;

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
