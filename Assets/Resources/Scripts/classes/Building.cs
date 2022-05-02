using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

[Serializable]
public class Building
{
    public string Name { get; }
    public BigInteger BaseCost { get; }
    public string ColorHex { get; }
    public double BaseLps { get; }
    public string Logo { get; }

    public int Version { get; private set; }
    public int Count { get; private set; }
    public BigInteger CurrentCost { get; private set; }
    public string Color { get; private set; }

    public Building(string name, BigInteger baseCost, string colorHex, float baseLps, string logo)
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
    }

    public void Upgrade()
    {
        ++Version;
    }

    public void Buy()
    {
        ++Count;
        SetCurrentCost();
    }

    private void SetCurrentCost()
    {
        CurrentCost = BigInteger.Multiply(BaseCost, new BigInteger(Math.Pow(1.15f, Count)));
    }

    public double CalculateLps()
    {
        var lps = BaseLps * Count;
        lps *= Mathf.Pow(2, Version);
        return lps;
    }

    public GameObject InstantiateGameObject(GameObject building)
    {
        var logo = Resources.Load<Sprite>(Logo);

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

        return building;
    }
}