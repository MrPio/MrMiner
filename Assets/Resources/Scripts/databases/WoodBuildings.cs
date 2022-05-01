using UnityEngine;
using System.Collections.Generic;
using System.Numerics;


public class WoodBuildings
{
    public static readonly Building[] Buildings = {
        new("Metal Axe", BigInteger.Parse("15"), "#FFFF6D", .1f, "Sprites/BuildingsWood/build_axe"),
        new("Chainsaw", BigInteger.Parse("100"), "#FFC578", 1f, "Sprites/BuildingsWood/build_chainsaw"),
        new("Sawmill", BigInteger.Parse("1100"), "#86FF96", 8f, "Sprites/BuildingsWood/build_sawmill")
    };
}