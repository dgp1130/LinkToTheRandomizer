﻿using DevelWithoutACause.Randomizer;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public class LogicGraphFactory
{
    /** Returns a LogicGraph from a LogicFile. */
    public static LogicGraph From(LogicFile logic)
    {
        // Get a complete set of nodes, keyed by name.
        var startNode = LogicNode.From("Start");
        var allNodes = logic.Checks!.Select((pair) => LogicNode.From(name: pair.Key))
                .Concat(new List<LogicNode> { startNode })
                .ToDictionary((node) => node.Name);

        // Get a complete set of keys, keyed by name.
        var allKeys = logic.Checks!.SelectMany(
            (pair) => (pair.Value ?? new List<LogicCheckEntry> { }).SelectMany(
                (check) => check.Keys.Select((keyName) => LogicKey.From(keyName))
            )
        ).Distinct()
        .ToDictionary((key) => key.Name);

        // Get a complete set of all the edges.
        var edges = logic.Checks!.SelectMany(
            (pair) => (pair.Value ?? new List<LogicCheckEntry> { }).Select((check) => {
                var start = check.From != null ? allNodes[check.From] : startNode;
                var end = allNodes[pair.Key];
                var keys = check.Keys.Select((keyName) => allKeys[keyName])
                        .ToImmutableHashSet();
                return LogicEdge.From(
                    start: start,
                    end: end,
                    keys: keys
                );
            })
        );

        // Construct the graph from all the parsed nodes and edges.
        return LogicGraph.From(
            nodes: allNodes.Values.ToImmutableHashSet(),
            edges: edges.ToImmutableHashSet(),
            start: startNode,
            end: allNodes["End"]
        );
    }
}