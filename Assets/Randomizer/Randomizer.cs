#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DevelWithoutACause.Randomizer
{
    /// <summary>
    /// The randomization algorithm works as follows:
    /// 
    /// 1.  Find all checks accessible from game start with the initial set of keys.
    /// 2.  Find all "boundary locks". This is every "door" which is reachable *but not unlockable*.
    /// ASSERTION: For all locations to be reachable, at least one key for a boundary lock *must* be
    ///     present in the currently accessible locations.
    ///     *   If this were false, all key items could be placed in a location inaccessible to the
    ///         player, meaning the player could make no progress and the seed would be unbeatable.
    /// 3.  Based on this assertion, pick a random key which unlocks a random boundary lock and place
    ///     it at any accessible check.
    /// 4.  Player now has access to new key, unlock all boundary locks that can now be opened (and
    ///     new boundary locks revealed by opening the initial boundary locks).
    /// 5.  Repeat until all locations are accessible (set of boundary locks is empty).
    /// 6.  All locations are now reachable with the set of keys currently placed on the graph. All
    ///     other checks are optional and are filled randomly with items remaining in the pool.
    /// 7.  Once the pool is empty, fill any remaining checks with junk.
    /// 8.  All checks are now filled, all locations are proven to be accessible (based on the items
    ///     placed before step 6), and the randomization is complete.
    /// </summary>
    public sealed class Randomizer
    {
        /**
         * Returns a randomized version of the given `LogicGraph`. Keys required by the graph are
         * placed randomly throughout in a logically solvable fashion. `initialKeys` is the set of
         * keys a player starts with, while the `seed` is the seed for the random number generator
         * used during the process. The returned `LogicGraph` has identical node and edge layout,
         * however nodes will now include keys. Unused nodes are left with no key and may be filled
         * with junk at runtime.
         */
        public static LogicGraph Randomize(
            LogicGraph graph,
            ImmutableSortedSet<LogicKey> initialKeys,
            int seed
        ) {
            var rng = new Random(seed);

            // Create pools of all nodes and keys.
            var nodePool = Pool<LogicNode>.From(graph.Nodes);
            var allKeys = graph.Edges
                .SelectMany((edge) => edge.Keys)
                .Distinct()
                .ToImmutableList();
            var keyPool = Pool<LogicKey>.From(allKeys);

            // Start with the input state.
            var startNodes = ImmutableSortedSet.Create(graph.Start);
            var state = new RandomizationState { Graph = graph, AccessibleKeys = initialKeys };
            do
            {
                // Place one key in an accessible location which makes progress towards
                // unlocking a currently inaccessible location.
                state = placeKey(
                    state: state,
                    startNodes: startNodes,
                    nodePool: nodePool,
                    keyPool: keyPool,
                    rng: rng
                );

                // Stop once all locations are reachable.
            } while (!allLocationsReachable(state.Graph, startNodes, state.AccessibleKeys));

            // All locations are now reachable, but there may be some optional keys left in the
            // pool. Place them all randomly.
            while (!keyPool.Empty)
            {
                state = placeKey(
                    state: state,
                    startNodes: startNodes,
                    nodePool: nodePool,
                    keyPool: keyPool,
                    rng: rng
                );
            }

            // All locations are now reachable, so this is completable in-logic. Note that
            // some locations may still be empty as they are not required.
            return state.Graph;
        }
        
        /**
         * Takes a current state of randomization (partially filled graph and available keys)
         * and places one key in a logically useful location in the graph. A "logically useful
         * key location" in this context is defined as a key which makes progress towards a
         * location not currently accessible and is placed in a location that is currently
         * accessible.
         */
        private static RandomizationState placeKey(
            RandomizationState state,
            ImmutableSortedSet<LogicNode> startNodes,
            Pool<LogicNode> nodePool,
            Pool<LogicKey> keyPool,
            Random rng
        ) {
            // Find all locations currently accessible with the given keys.
            var accessibleNodesEnumerable = findAccessibleNodesFrom(
                graph: state.Graph,
                startNodes: startNodes,
                keys: state.AccessibleKeys
            );
            var accessibleNodes = accessibleNodesEnumerable.ToImmutableSortedSet();

            // Find all boundary locks. These are "doors" which are accessible to the player
            // but not yet unlockable becuase of missing keys.
            var boundaryLocks = getBoundaryEdges(state.Graph, accessibleNodes);

            // Find all the unique keys used in the boundary locks which will make meaningful
            // progress if given to the player now.
            var boundaryKeys = boundaryLocks
                .SelectMany((edge) => edge.Keys)
                .Distinct();

            // Find all the empty, accessible, checkable locations a key can be safely placed
            // where the player can retrieve it.
            var availableNodes = accessibleNodes
                .Where((node) => node.Checkable && node.Check == null);

            // Pick a node and key at random.
            var node = nodePool.TakeRandomFrom(rng, availableNodes);
            var key = boundaryKeys.Count() != 0
                ? keyPool.TakeRandomFrom(rng, boundaryKeys) // Pick a boundary key.
                : keyPool.TakeRandom(rng); // No boundary keys remaining, just pick any key.

            // Transform the graph to place the single key at the given node and grant the
            // player the new key.
            return new RandomizationState
            {
                Graph = state.Graph.Place(node, key),
                AccessibleKeys = state.AccessibleKeys
                    .Concat(ImmutableList.Create(key))
                    .ToImmutableSortedSet(),
            };
        }

        /**
         * Returns all the nodes accessible from the given starting locations with the
         * current set of keys.
         */
        private static IEnumerable<LogicNode> findAccessibleNodesFrom(
            LogicGraph graph,
            ImmutableSortedSet<LogicNode> startNodes,
            ImmutableSortedSet<LogicKey> keys
        ) {
            return findAccessibleNodesFrom(
                graph: graph,
                startNodes: startNodes,
                availableNodes: startNodes,
                keys: keys
            );
        }
        
        /**
         * Returns all the nodes accessible from the given starting locations with the
         * current set of keys. Ignores nodes which are already available to the player.
         */
        private static IEnumerable<LogicNode> findAccessibleNodesFrom(
            LogicGraph graph,
            ImmutableSortedSet<LogicNode> startNodes,
            ImmutableSortedSet<LogicNode> availableNodes,
            ImmutableSortedSet<LogicKey> keys
        ) {
            // TODO: Optimize by skipping nodes which have two edges pointing to them and
            // were already checked.
            // TODO: Index edges by start/end node?
            return startNodes
                // Find all edges which start at an accessible node and are unlockeable.
                .SelectMany((node) => graph.Edges
                    .Where((edge) => edge.Start == node)
                    .Where((edge) => edge.UnlockableWith(keys))
                )
                // Ignore backwards edges that visit nodes we already have access to.
                .Where((edge) => !availableNodes.Contains(edge.End))
                // Recursively find all accessible nodes for each newly accessible node.
                .SelectMany((edge) => findAccessibleNodesFrom(
                    graph: graph,
                    startNodes: ImmutableSortedSet.Create(edge.End),
                    availableNodes: availableNodes.Concat(ImmutableList.Create(edge.End))
                        .ToImmutableSortedSet(),
                    keys: keys
                ))
                // Combine with nodes already known to be accessible to the player. No need
                // to deduplicate because backwards edges to previously accessible location
                // were already filtered out.
                .Concat(availableNodes);
        }

        /**
         * Returns all the "boundary edges" in the graph. This is defined as the edges which
         * are reachable but locked to the player due to a lack of keys.
         */
        private static IEnumerable<LogicEdge> getBoundaryEdges(
            LogicGraph graph,
            IEnumerable<LogicNode> nodes
        ) {
            return graph.Edges
                .Where((edge) => nodes.Contains(edge.Start) && !nodes.Contains(edge.End));
        }

        /** Returns whether or not all locations in the graph are reachable by the player. */
        private static bool allLocationsReachable(
            LogicGraph graph,
            ImmutableSortedSet<LogicNode> startNodes,
            ImmutableSortedSet<LogicKey> keys
        ) {
            var accessibleNodes = findAccessibleNodesFrom(
                graph: graph,
                startNodes: startNodes,
                keys: keys
            );
            var boundaryEdges = getBoundaryEdges(graph, accessibleNodes);
            return boundaryEdges.Count() == 0;
        }

        /** Internal data type representing the current state of the randomization process. */
        private sealed class RandomizationState
        {
            /** The current graph with some keys placed. */
            public LogicGraph Graph { get; set; } = null!;

            /** Keys currently accessible to the player with the current graph state. */
            public ImmutableSortedSet<LogicKey> AccessibleKeys { get; set; } = null!;
        }
    }
}
