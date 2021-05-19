#nullable enable

using System;
using System.Collections.Immutable;
using System.Linq;

namespace DevelWithoutACause.Randomizer
{
    /** Represents a graph of Nodes, with Edge objects connecting them. */
    public class LogicGraph
    {
        public readonly ImmutableSortedSet<LogicNode> Nodes;
        public readonly ImmutableSortedSet<LogicEdge> Edges;
        public readonly LogicNode Start;
        public readonly LogicNode End;

        private LogicGraph(
            ImmutableSortedSet<LogicNode> nodes,
            ImmutableSortedSet<LogicEdge> edges,
            LogicNode start,
            LogicNode end
        ) {
            Nodes = nodes;
            Edges = edges;
            Start = start;
            End = end;
        }

        /**
         * Returns a graph of the given nodes and edges with the provided
         * start and end positions. The player is trying to move from the start
         * node to the end node while legally following all the edges of the graph.
         */
        public static LogicGraph From(
            ImmutableSortedSet<LogicNode> nodes,
            ImmutableSortedSet<LogicEdge> edges,
            LogicNode start,
            LogicNode end
        ) {
            return new LogicGraph(
                nodes: nodes,
                edges: edges,
                start: start,
                end: end
            );
        }

        /**
         * Returns a new `LogicGraph` identical to `this` one, but with the given `node`
         * transformed to contain the given `key`.
         */
        public LogicGraph Place(LogicNode node, LogicKey key)
        {
            var replacement = node.Place(key);

            var nodes = Nodes
                .Remove(node)
                .Concat(ImmutableList.Create(replacement))
                .ToImmutableSortedSet();
            var edges = Edges
                .Select((edge) => edge.Start == node ? edge.ReplaceStart(replacement) : edge)
                .Select((edge) => edge.End == node ? edge.ReplaceEnd(replacement) : edge)
                .ToImmutableSortedSet();
            var start = Start == node ? replacement : Start;
            var end = End == node ? replacement : End;

            return new LogicGraph(
                nodes: nodes,
                edges: edges,
                start: start,
                end: end
            );
        }

        public override string ToString()
        {
            return $"{{\n{string.Join("\n", Edges.Select((edge) => $"  {edge}"))}\n}}";
        }
    }

    /** Represents a named location which will hold a check. */
    public class LogicNode : IComparable<LogicNode>
    {
        /** A label for the node. */
        public readonly string Name;

        /** Whether a key can be placed at this location. */
        public readonly bool Checkable;

        /** The key currently placed at this location. */
        public readonly LogicKey? Check;

        private LogicNode(string name, bool checkable, LogicKey? check)
        {
            Name = name;
            Checkable = checkable;
            Check = check;
        }

        public static LogicNode From(string name, bool checkable, LogicKey? check = null)
        {
            return new LogicNode(
                name: name,
                checkable: checkable,
                check: check
            );
        }

        /** Returns a new `LogicNode` identical to `this` one but holding the given `key`. */
        public LogicNode Place(LogicKey key)
        {
            if (Check != null)
            {
                throw new InvalidOperationException(
                    $"Cannot place a key on a check that is already set with: {key}");
            }

            return new LogicNode(
                name: Name,
                checkable: Checkable,
                check: key
            );
        }

        public int CompareTo(LogicNode other)
        {
            return Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return $"{Name} ({Check?.ToString() ?? "Empty"})";
        }
    }

    /**
     * Represents an edge which connects two nodes with a required set of keys.
     * If the player is at the start node with *all* of the required keys, then
     * they also logically have access to the end node.
     */
    public class LogicEdge : IComparable<LogicEdge>
    {
        public readonly LogicNode Start;
        public readonly LogicNode End;
        public readonly ImmutableSortedSet<LogicKey> Keys;

        private LogicEdge(LogicNode start, LogicNode end, ImmutableSortedSet<LogicKey> keys)
        {
            Start = start;
            End = end;
            Keys = keys;
        }
        
        public static LogicEdge From(LogicNode start, LogicNode end, ImmutableSortedSet<LogicKey> keys)
        {
            return new LogicEdge(
                start: start,
                end: end,
                keys: keys
            );
        }

        /** Returns whether or not this edge can be unlocked by the given set of keys. */
        public bool UnlockableWith(ImmutableSortedSet<LogicKey> keys)
        {
            return Keys.IsSubsetOf(keys);
        }

        /** Returns a new `LogicEdge` identical to `this` one, but with the start node replaced. */
        public LogicEdge ReplaceStart(LogicNode replacement)
        {
            return new LogicEdge(
                start: replacement,
                end: End,
                keys: Keys
            );
        }

        /** Returns a new `LogicEdge` identical to `this` one, but with the end node replaced. */
        public LogicEdge ReplaceEnd(LogicNode replacement)
        {
            return new LogicEdge(
                start: Start,
                end: replacement,
                keys: Keys
            );
        }

        public int CompareTo(LogicEdge other)
        {
            // First compare the start nodes.
            var startComparison = Start.CompareTo(other.Start);
            if (startComparison != 0) return startComparison;

            // Next compare the end nodes.
            var endComparison = End.CompareTo(other.End);
            if (endComparison != 0) return endComparison;

            // Next compare the number of keys.
            var keyLengthComparison = Keys.Count.CompareTo(other.Keys.Count);
            if (keyLengthComparison != 0) return keyLengthComparison;

            // Finally, compare the keys themselves. Since this is a sorted set,
            // equivalent sets should be in the same order.
            return Keys.Zip(other.Keys, (key, otherKey) => key.CompareTo(otherKey))
                .FirstOrDefault((keyComparison) => keyComparison != 0);
        }

        public override string ToString()
        {
            return $"{Start} -> {End} ({string.Join(", ", Keys)})";
        }
    }

    /**
     * A named key object which helps the player access some nodes. Note that a Key
     * object may not actually be a physical key, but could be an item which helps
     * the player access a particular area.
     */
    public class LogicKey : IComparable<LogicKey>
    {
        public readonly string Name;

        private LogicKey(string name)
        {
            Name = name;
        }

        public static LogicKey From(string name)
        {
            return new LogicKey(name: name);
        }

        public override bool Equals(object obj)
        {
            if (obj?.GetType() != this.GetType()) return false;

            var other = obj as LogicKey;
            return this.Name == other?.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public int CompareTo(LogicKey other)
        {
            return Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
