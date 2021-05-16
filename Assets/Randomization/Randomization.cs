#nullable enable

using DevelWithoutACause.Randomizer;
using System.Collections.Immutable;
using UnityEngine;

[CreateAssetMenu(menuName = "LinkToTheRandomizer/Randomization")]
public class Randomization : ScriptableObject
{
    [SerializeField] int seed;
    [SerializeField] TextAsset? logicFile;

    private ImmutableDictionary<Check, Item> randomizedItems = null!;

    public void OnEnable()
    {
        var logic = LogicFile.Deserialize(logicFile!.text);
        var logicGraph = LogicGraphFactory.From(logic);
        randomizedItems = Randomizer.Randomize(seed, logicGraph);
    }


    public Item GetItemForCheck(Check check)
    {
        return randomizedItems[check];
    }
}
