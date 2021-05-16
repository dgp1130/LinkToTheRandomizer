using System.Collections.Immutable;

namespace DevelWithoutACause.Randomizer
{
    /** Represents a graph of Nodes, with Edge objects connecting them. */
    public class LogicGraph
    {
        private readonly ImmutableHashSet<LogicNode> nodes;
        private readonly ImmutableHashSet<LogicEdge> edges;
        private readonly LogicNode start;
        private readonly LogicNode end;

        private LogicGraph(
            ImmutableHashSet<LogicNode> nodes,
            ImmutableHashSet<LogicEdge> edges,
            LogicNode start,
            LogicNode end
        ) {
            this.nodes = nodes;
            this.edges = edges;
            this.start = start;
            this.end = end;
        }

        /**
         * Returns a graph of the given nodes and edges with the provided
         * start and end positions. The player is trying to move from the start
         * node to the end node while legally following all the edges of the graph.
         */
        public static LogicGraph From(
            ImmutableHashSet<LogicNode> nodes,
            ImmutableHashSet<LogicEdge> edges,
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
    }

    /** Represents a named location which will hold a check. */
    public class LogicNode
    {
        public readonly string Name;

        private LogicNode(string name)
        {
            this.Name = name;
        }

        public static LogicNode From(string name)
        {
            return new LogicNode(
                name: name
            );
        }
    }

    /**
     * Represents an edge which connects two nodes with a required set of keys.
     * If the player is at the start node with *all* of the required keys, then
     * they also logically have access to the end node.
     */
    public class LogicEdge
    {
        private readonly LogicNode start;
        private readonly LogicNode end;
        private readonly ImmutableHashSet<LogicKey> keys;

        private LogicEdge(LogicNode start, LogicNode end, ImmutableHashSet<LogicKey> keys)
        {
            this.start = start;
            this.end = end;
            this.keys = keys;
        }

        public static LogicEdge From(LogicNode start, LogicNode end, ImmutableHashSet<LogicKey> keys)
        {
            return new LogicEdge(
                start: start,
                end: end,
                keys: keys
            );
        }
    }

    /**
     * A named key object which helps the player access some nodes. Note that a Key
     * object may not actually be a physical key, but could be an item which helps
     * the player access a particular area.
     */
    public class LogicKey
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
            if (obj.GetType() != this.GetType()) return false;

            var other = obj as LogicKey;
            return this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
