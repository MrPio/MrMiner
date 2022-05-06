using System;
using System.Numerics;

[Serializable]
public class Store
{
    public int Version { get; private set; }
    public int Count { get; private set; }
    private string Color { get; }
    private BigInteger UpgradeBaseCost { get; }
    public bool buildingAvailable, upgradeAvailable;

    private string Name { get; }
    private BigInteger BaseCost { get; }
    private double BaseCps { get; }
    private string Logo { get; }

    public Store(string name, BigInteger baseCost, string colorHex, float baseCps, string logo,
        BigInteger upgradeBaseCost)
    {
        Name = name;
        BaseCost = baseCost;
        BaseCps = baseCps;
        Logo = logo;
        Version = 0;
        Count = 0;
        Color = colorHex;
        UpgradeBaseCost = upgradeBaseCost;
    }

    public double Cps => 0;
}