using DevelWithoutACause.Randomizer;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LinkToTheRandomizer/Randomization")]
public class Randomization : ScriptableObject
{
    private readonly Dictionary<Check, Item> randomizedItems = Randomizer.Randomize(1);

    public Item GetItemForCheck(Check check)
    {
        return randomizedItems[check];
    }
}
