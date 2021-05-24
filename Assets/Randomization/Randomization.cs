#nullable enable

using DevelWithoutACause.Randomizer;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "LinkToTheRandomizer/Randomization")]
public sealed class Randomization : ScriptableObject
{
    [SerializeField] int seed;
    [SerializeField] TextAsset? logicFile;

    private ImmutableDictionary<Check, Item> randomizedItems = null!;

    public void OnEnable()
    {
        var knownChecks = (Enum.GetValues(typeof(Check)) as Check[])
            .Select((check) => new KeyValuePair<string, Check>(
                Enum.GetName(typeof(Check), check),
                check
            ))
            .ToImmutableDictionary();
        var allItems = (Enum.GetValues(typeof(Item)) as Item[])
            .Select((item) => new KeyValuePair<string, Item>(
                Enum.GetName(typeof(Item), item),
                item
            ))
            .ToImmutableDictionary();

        // Parse the logic YAML file into a graph.
        var logic = LogicFile.Deserialize(logicFile!.text);
        var logicGraph = LogicGraphFactory.From(logic);

        // Randomly place items in the graph in a logically solvable fashion.
        var randomizedGraph = Randomizer.Randomize(
            graph: logicGraph,
            initialKeys: ImmutableSortedSet<LogicKey>.Empty,
            seed: seed
        );
        
        // Transform the graph into a `Dictionary<Check, Item>`, which is useful
        // for the game at runtime.
        randomizedItems = randomizedGraph.Nodes
            .Where((node) => knownChecks.ContainsKey(node.Name))
            .Select((node) => new KeyValuePair<Check, Item>(
                knownChecks[node.Name],
                node.Check != null ? allItems[node.Check.Name] : Item.Bluepee
            ))
            .ToImmutableDictionary();
    }


    public Item GetItemForCheck(Check check)
    {
        return randomizedItems[check];
    }
}
