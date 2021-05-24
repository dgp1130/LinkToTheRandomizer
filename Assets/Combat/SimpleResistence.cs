#nullable enable

using System;
using UnityEngine;

/**
 * <summary>
 * Defines a "simple" resistence to a particular damage type.
 * 
 * "Simple" in this case just means there is a flat resistence value which is subtracted
 * from the incoming damage value.
 * </summary>
 */
[CreateAssetMenu(menuName = "LinkToTheRandomizer/SimpleResistence")]
public sealed class SimpleResistence : Resistence
{
    [SerializeField] public int Defense;

    public override int Resist(int damage)
    {
        return Math.Max(damage - Defense, 0);
    }
}
