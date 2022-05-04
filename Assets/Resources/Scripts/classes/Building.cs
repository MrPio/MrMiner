using System;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

//[Serializable]
public class Building
{
    public int Version { get; private set; }
    public int Count { get; private set; }
    private string Color { get; }
    private BigInteger UpgradeBaseCost { get; }
    public bool BuildingAvailable, UpgradeAvailable;

    private string Name { get; }
    private BigInteger BaseCost { get; }
    private double BaseLps { get; }
    private string Logo { get; }

    public Building(string name, BigInteger baseCost, string colorHex, float baseLps, string logo,
        BigInteger upgradeBaseCost)
    {
        Name = name;
        BaseCost = baseCost;
        BaseLps = baseLps;
        Logo = logo;
        Version = 0;
        Count = 0;
        Color = colorHex;
        UpgradeBaseCost = upgradeBaseCost;
    }

    public void Upgrade()
    {
        ++Version;
    }

    public bool CheckForUpgrade()
    {
        var requirements = Name == "Mighty Fist" ? WoodBuildings.RequiredProgressFist : WoodBuildings.RequiredProgress;
        var upgradesAvailable = requirements.Count(requirement => Count >= requirement);
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
    }

    public  BigInteger CurrentCost => new((double) BaseCost * Math.Pow(1.15f, Count));

    public double Lps => BaseLps * Count*Mathf.Pow(2, Version);

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
            utilies.NumToStr(CurrentCost);
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