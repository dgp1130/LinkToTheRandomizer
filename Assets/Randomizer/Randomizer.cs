using System;
using System.Collections.Generic;

namespace DevelWithoutACause.Randomizer
{
    public class Randomizer
    {
        public static Dictionary<Check, Item> Randomize(int seed)
        {
            // Create a pool of all items.
            var allItems = new List<Item>(Enum.GetValues(typeof(Item)) as Item[]);
            var pool = Pool<Item>.From(allItems);

            // Simplest possible algorithm for now, just pick a random item for every check.
            var rng = new Random(seed);
            return new Dictionary<Check, Item>()
            {
                { Check.Left, pool.TakeRandom(rng) },
                { Check.Middle, pool.TakeRandom(rng) },
                { Check.Right, pool.TakeRandom(rng) },
            };
        }
    }
}
