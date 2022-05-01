using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class Building:MonoBehaviour
{
    public string Name { get; }
    public BigInteger BaseCost { get; }
    public string ColorHex { get; }
    public float BasePS { get; }
    public Sprite Logo { get; }

    public int Version { get; private set; }
    public int Count { get; private set; }
    public BigInteger CurrentCost { get; private set; }
    public Color Color { get; private set; }

    public Building(string name, BigInteger baseCost, string colorHex, float basePS, string logo)
    {
        Name = name;
        BaseCost = baseCost;
        ColorHex = colorHex;
        BasePS = basePS;
        Logo = Resources.Load<Sprite>(logo);
        Version = 1;
        Count = 0;
        SetCurrentCost();
        Color = utilies.HexToColor(colorHex);
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

    public GameObject InstantiateGameObject(Transform parent)
    {
        var building = Instantiate(Resources.Load<GameObject>("Prefabs/ShopItem"), parent, false);

        var shopBase = building.transform.Find("ShopItem_base").gameObject;
        shopBase.GetComponent<Image>().color = Color;
        shopBase.transform.Find("ShopItem_name").GetComponent<TextMeshProUGUI>().text = Name;
        shopBase.transform.Find("ShopItem_price").GetComponent<TextMeshProUGUI>().text = CurrentCost.ToString();
        shopBase.transform.Find("ShopItem_value").GetComponent<TextMeshProUGUI>().text = Count.ToString();
        shopBase.transform.Find("ShopItem_logo").GetComponent<Image>().sprite = Logo;
        var xScaleFactor = Logo.bounds.size.x / Logo.bounds.size.y/0.942445993f;
        var rect = shopBase.transform.Find("ShopItem_logo").GetComponent<RectTransform>().rect;
        rect.width *= xScaleFactor;
        shopBase.transform.Find("ShopItem_logo").GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width,rect.height);

        return building;
    }
}