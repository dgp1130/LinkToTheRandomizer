using DevelWithoutACause.Randomizer;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LinkToTheRandomizer/Randomization")]
public class Randomization : ScriptableObject
{
    [SerializeField] int seed;

    private Dictionary<Check, Item> randomizedItems = null!;

    public void OnEnable()
    {
        randomizedItems = Randomizer.Randomize(seed);
    }


    public Item GetItemForCheck(Check check)
    {
        return randomizedItems[check];
    }
}
