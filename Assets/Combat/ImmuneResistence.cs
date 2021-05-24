#nullable enable

﻿using UnityEngine;

/** Resistence which blocks all incoming damage from the associated type. */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/ImmuneResistence")]
public sealed class ImmuneResistence : Resistence
{
    public override int Resist(int damage)
    {
        // Immune to all damage of this type.
        return 0;
    }
}
