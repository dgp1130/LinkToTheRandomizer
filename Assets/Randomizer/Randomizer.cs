using System;
using System.Collections.Generic;

namespace DevelWithoutACause.Randomizer
{
    public class Randomizer
    {
        public static Dictionary<Check, Item> Randomize(int seed)
        {
            // Simplest possible algorithm for now, just pick a random item for every check.
            var rng = new Random(seed);
            return new Dictionary<Check, Item>()
            {
                { Check.Left, RandomItem(rng) },
                { Check.Middle, RandomItem(rng) },
                { Check.Right, RandomItem(rng) },
            };
        }

        private static Item RandomItem(Random rng)
        {
            return rng.Next(3) switch
            {
                0 => Item.Sword,
                1 => Item.Bow,
                2 => Item.Bomb,
                var val => throw new Exception($"Unknown item for value {val}"),
            };
        }
    }
}
