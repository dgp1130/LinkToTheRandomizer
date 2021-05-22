#nullable enable

using System;
using UnityEngine;

/** Simple enum representing the cardinal directions in a 2D plane. */
public enum Direction
{
    North,
    South,
    East,
    West,
}

public static class DirectionExtensions
{
    public static Vector2 ToVector(this Direction direction)
    {
        return direction switch
        {
            Direction.North => Vector2.up,
            Direction.South => Vector2.down,
            Direction.East => Vector2.right,
            Direction.West => Vector2.left,
            _ => throw new ArgumentException($"Unknown direction: {direction}."),
        };
    }
}