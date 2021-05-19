using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DevelWithoutACause.Randomizer
{
    /**
     * Represents a "pool" of values. Each time a value is taken, it is removed from the
     * pool and will *not* be returned again.
     */
    class Pool<T>
    {
        private readonly List<T> pool;

        private Pool(List<T> pool)
        {
            this.pool = pool;
        }

        /** Returns a new pool starting with the given set of values. */
        public static Pool<T> From(IEnumerable<T> initialPool) {
            // Copy the list so we don't mutate external data.
            return new Pool<T>(pool: initialPool.ToList());
        }

        private T take(int index)
        {
            var value = pool[index];
            pool.RemoveAt(index);
            return value;
        }

        /** Returns a random value currently in the pool and removes it. */
        public T TakeRandom(Random rng)
        {
            if (pool.Count == 0) throw new InvalidOperationException($"No more items left in pool.");

            // Get and remove a random item from the pool.
            return take(rng.Next(pool.Count));
        }

        /** Returns and removes a random value currently in the pool and within the provided collection. */
        public T TakeRandomFrom(Random rng, IEnumerable<T> collection)
        {
            var indexes = collection
                .Select((item) => pool.FindIndex((i) => EqualityComparer<T>.Default.Equals(i, item)))
                .Where((index) => index != -1)
                .ToImmutableList();

            if (indexes.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No values in the pool match requested collection: {string.Join(", ", collection)}");
            }

            return take(indexes[rng.Next(indexes.Count)]);
        }
    }
}
