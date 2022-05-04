using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

//[Serializable]
public class Building
{
    private static int[] _upgradeCostMultiplier = {1, 5, 10, 100, 100, 100, 1000, 1000, 1000, 1000, 10000};
    private static int[] _upgradeCostMultiplierFist = {1, 5, 20, 10, 100, 10, 10, 10, 1000, 1000, 1000, 1000};
    private static int[] _upgradeRequired = {1, 5, 25, 50, 100, 150, 200, 250, 300, 350, 400};
    private static int[] _upgradeRequiredFist = {1, 1, 5, 25, 50, 100, 150, 200, 250, 300, 350, 400};
    public string Name { get; }
    public BigInteger BaseCost { get; }
    public string ColorHex { get; }
    public double BaseLps { get; }
    public string Logo { get; }

    public int Version { get; private set; }
    public int Count { get; private set; }
    public BigInteger CurrentCost { get; private set; }
    public string Color { get; private set; }
    public BigInteger UpgradeBaseCost { get; }

    public bool available;

    public Building(string name, BigInteger baseCost, string colorHex, float baseLps, string logo,
        BigInteger upgradeBaseCost)
    {
        Name = name;
        BaseCost = baseCost;
        ColorHex = colorHex;
        BaseLps = baseLps;
        Logo = logo;
        Version = 0;
        Count = 0;
        SetCurrentCost();
        Color = colorHex;
        UpgradeBaseCost = upgradeBaseCost;
    }

    public void Upgrade()
    {
        ++Version;
    }

    public bool CheckForUpgrade()
    {
        var upgradesAvailable = 0;
        var requirements = Name == "Mighty Fist" ? WoodBuildings.RequiredProgressFist : WoodBuildings.RequiredProgress;
        foreach (var requirement in requirements)
            if (upgradesAvailable >= requirement)
                ++upgradesAvailable;
        return upgradesAvailable > Version;
    }

    public BigInteger CalculateUpgradeCost()
    {
        var cost = UpgradeBaseCost;
        var costs = Name == "Mighty Fist" ? WoodBuildings.CostMultiplyFist : WoodBuildings.CostMultiply;
        for (int i = 0; i < Version + 1; i++)
            cost = BigInteger.Multiply(cost, new BigInteger(costs[i]));
        return cost;
    }

    public void Buy()
    {
        ++Count;
        SetCurrentCost();
    }

    private void SetCurrentCost()
    {
        CurrentCost = new BigInteger((double) BaseCost * Math.Pow(1.15f, Count));
    }

    public double CalculateLps()
    {
        var lps = BaseLps * Count;
        lps *= Mathf.Pow(2, Version);
        return lps;
    }

    public GameObject InstantiateGameObject(GameObject building, int index)
    {
        var logo = Resources.Load<Sprite>(Logo);

        building.GetComponent<ShopItem>().index = index;
        building.tag = "ShopItem";

        var shopBase = building.transform.Find("ShopItem_base").gameObject;
        shopBase.GetComponent<Image>().color = utilies.HexToColor(Color);
        if (Color == "#545454")
        {
            shopBase.transform.Find("ShopItem_value").GetComponent<TextMeshProUGUI>().color =
                utilies.HexToColor("#AEA1AE");
            shopBase.transform.Find("ShopItem_name").GetComponent<TextMeshProUGUI>().color =
                utilies.HexToColor("#9F8D9F");
        }

        shopBase.transform.Find("ShopItem_name").GetComponent<TextMeshProUGUI>().text = Name;
        shopBase.transform.Find("ShopItem_price").GetComponent<TextMeshProUGUI>().text =
            utilies.NumberToFormattedString(CurrentCost);
        shopBase.transform.Find("ShopItem_value").GetComponent<TextMeshProUGUI>().text = Count.ToString();
        shopBase.transform.Find("ShopItem_logo").GetComponent<Image>().sprite = logo;
        var xScaleFactor = logo.bounds.size.x / logo.bounds.size.y / 0.942445993f;
        var rect = shopBase.transform.Find("ShopItem_logo").GetComponent<RectTransform>().rect;
        rect.width *= xScaleFactor;
        shopBase.transform.Find("ShopItem_logo").GetComponent<RectTransform>().sizeDelta =
            new Vector2(rect.width, rect.height);

        shopBase.transform.Find("ShopItem_version").GetComponent<TextMeshProUGUI>().text = "lv." + Version;

        return building;
    }
}