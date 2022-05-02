using UnityEngine;
using System.Collections.Generic;
using System.Numerics;


public class WoodBuildings
{
    public static readonly Building[] Buildings = {
        new("Mighty Fist", BigInteger.Parse("15"), "#FFFF6D", .1f, "Sprites/BuildingsWood/1_Fist"),
        new("Metal Axe", BigInteger.Parse("100"), "#FFC578", 1f, "Sprites/BuildingsWood/2_Axe"),
        new("Chainsaw", BigInteger.Parse("1100"), "#86FF96", 8f, "Sprites/BuildingsWood/3_Chainsaw"),
        new("Woodsman", BigInteger.Parse("12000"), "#84FFF4", 47f, "Sprites/BuildingsWood/4_Woodsman"),
        new("Wooden House", BigInteger.Parse("130000"), "#83B6FF", 260f, "Sprites/BuildingsWood/5_WoodenHouse"),
        new("Sawmill", BigInteger.Parse("1400000"), "#CC92FF", 1400f, "Sprites/BuildingsWood/6_Sawmill"),
        new("GoldenTree", BigInteger.Parse("20000000"), "#FFEC72", 7800f, "Sprites/BuildingsWood/7_GoldenTree"),
        new("Forest", BigInteger.Parse("330000000"), "#5EE694", 44000f, "Sprites/BuildingsWood/8_Forest"),
        new("Mountain", BigInteger.Parse("5100000000"), "#9CFFEF", 260000f, "Sprites/BuildingsWood/9_Mountain"),
        new("Corporation", BigInteger.Parse("75000000000"), "#A1AEBD", 1600000f, "Sprites/BuildingsWood/10_Corporation"),
        new("Spaceship", BigInteger.Parse("1000000000000"), "#F5F5F5", 10000000f, "Sprites/BuildingsWood/11_Spaceship"),
        new("Portal", BigInteger.Parse("14000000000000"), "#D96868", 65000000f, "Sprites/BuildingsWood/12_Portal"),
        new("Creator", BigInteger.Parse("170000000000000"), "#545454", 430000000f, "Sprites/BuildingsWood/13_Creator")
    };
}