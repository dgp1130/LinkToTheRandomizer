using System;
using System.Collections.Generic;

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
        public static Pool<T> From(List<T> initialPool)
        {
            // Copy the list so we don't mutate external data.
            return new Pool<T>(pool: new List<T>(initialPool));
        }

        /** Returns a random value currently in the pool and removes it. */
        public T TakeRandom(Random rng)
        {
            // Assert that the pool is not empty.
            if (pool.Count == 0) throw new InvalidOperationException($"No more items in pool.");

            // Get and remove a random item from the pool.
            var index = rng.Next(pool.Count);
            var value = pool[index];
            pool.RemoveAt(index);
            return value;
        }
    }
}
