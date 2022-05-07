using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

[Serializable]
public class User
{
    private static readonly string SaveFileLocation = "/MrMiner_User.dat";

    public BigInteger Logs { get; private set; }
    public BigInteger Coins { get; private set; }
    public DateTime ProfileCreated { get; }
    public DateTime LastAutosave { get; private set; }
    public List<Building> buildings;
    public List<Store> stores;
    [NonSerialized] public List<TextMeshProUGUI> ShopItemValueText = new(), ShopItemPriceText = new();

    private int _clickVersionLog, _clickVersionCoin;
    private int _lpsPerc, _cpsPerc;
    private double _lpsRest, _cpsRest;
    private static readonly int Bounce = Animator.StringToHash("Bounce");


    public User()
    {
        Logs = new BigInteger(0);
        Coins = new BigInteger(0);
        ProfileCreated = DateTime.Now;
        _lpsPerc = 0;
        _cpsPerc = 0;
        Logs = BigInteger.Zero;
        buildings = new List<Building>();
        stores = new List<Store>();
        foreach (var building in WoodBuildings.Buildings)
            buildings.Add(building);
        foreach (var store in CoinBuildings.Stores)
            stores.Add(store);
        _clickVersionLog = 0;
        _clickVersionCoin = 0;
    }

    public void Save()
    {
        LastAutosave = DateTime.Now;
        var path = Application.persistentDataPath + SaveFileLocation;
        if (File.Exists(path))
            File.Delete(path);
        var file = File.Create(path);
        new BinaryFormatter().Serialize(file, this);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public static User Load()
    {
        var path = Application.persistentDataPath + SaveFileLocation;
        if (!File.Exists(path))
            return null;
        var file = File.Open(path, FileMode.Open);
        var user = (User) new BinaryFormatter().Deserialize(file);
        file.Close();
        return user;
    }

    public double Lps => buildings.Sum(building => building.Lps);
    public double Cps => stores.Sum(store => store.Cps);

    public BigInteger ClickPowerLog
    {
        get
        {
            var clickPower = BigInteger.Add(
                BigInteger.Pow(new BigInteger(2), _clickVersionLog),
                new BigInteger(Lps * _lpsPerc)
            );
            clickPower = BigInteger.Add(clickPower, new BigInteger(1000));
            return clickPower;
        }
    }
    public BigInteger ClickPowerCoin
    {
        get
        {
            var clickPower = BigInteger.Add(
                BigInteger.Pow(new BigInteger(2), _clickVersionLog),
                new BigInteger(Cps * _cpsPerc)
            );
            clickPower = BigInteger.Add(clickPower, new BigInteger(1000));
            return clickPower;
        }
    }

    public void EarnLps(int fps)
    {
        Logs = BigInteger.Add(Logs, new BigInteger(Lps / fps + _lpsRest));
        _lpsRest += Lps / fps - Math.Truncate(Lps / fps);
        _lpsRest -= Math.Truncate(_lpsRest);
        UpdateUI();
    }

    public void EarnCps(int fps)
    {
        Coins = BigInteger.Add(Coins, new BigInteger(Cps / fps + _cpsRest));
        _cpsRest += Cps / fps - Math.Truncate(Cps / fps);
        _cpsRest -= Math.Truncate(_cpsRest);
        UpdateUI();
    }

    public void EarnClickLog()
    {
        Logs = BigInteger.Add(Logs, ClickPowerLog);
        UpdateUI();
    }

    public void EarnClickLog(BigInteger value)
    {
        Logs = BigInteger.Add(Logs, value);
        UpdateUI();
    }
    
    public void EarnClickCoin()
    {
        Coins = BigInteger.Add(Coins, ClickPowerCoin);
        UpdateUI();
    }

    public void EarnClickCoin(BigInteger value)
    {
        Coins = BigInteger.Add(Coins, value);
        UpdateUI();
    }


    public void UpdateUI()
    {
        GameObject.FindGameObjectWithTag("Header_log_value").GetComponent<TextMeshProUGUI>().text =
            utilies.NumToStr(Logs);
        GameObject.FindGameObjectWithTag("Header_lps").GetComponent<TextMeshProUGUI>().text =
            utilies.DoubleToStr(Lps) + " lps";

        GameObject.FindGameObjectWithTag("Header_coin_value").GetComponent<TextMeshProUGUI>().text =
            utilies.NumToStr(Coins);
        GameObject.FindGameObjectWithTag("Header_cps").GetComponent<TextMeshProUGUI>().text =
            utilies.DoubleToStr(Cps) + " cps";
    }

    public void UpdateBuildUI()
    {
        var count = 0;
        foreach (var building in buildings)
        {
            ShopItemValueText[count].text = building.Count.ToString();
            ShopItemPriceText[count++].text = utilies.NumToStr(building.CurrentCost);
        }
    }

    public bool Buy(Building building)
    {
        if (!EnoughForBuilding(buildings.IndexOf(building)))
            return false;

        Logs = BigInteger.Subtract(Logs, building.CurrentCost);
        GameObject.FindGameObjectWithTag("Header_lps").GetComponent<ColorFade>().FadeToColor(Color.white,
            utilies.HexToColor("#FF5C26"), typeof(TextMeshProUGUI));
        GameObject.FindGameObjectWithTag("Header_lps").GetComponent<Animator>().SetTrigger(Bounce);

        building.Buy();
        UpdateBuildUI();
        return true;
    }

    public bool Buy(Store store)
    {
        if (!EnoughForStore(stores.IndexOf(store)))
            return false;

        Coins = BigInteger.Subtract(Coins, store.CurrentCost);
        GameObject.FindGameObjectWithTag("Header_cps").GetComponent<ColorFade>().FadeToColor(Color.white,
            utilies.HexToColor("#FF5C26"), typeof(TextMeshProUGUI));
        GameObject.FindGameObjectWithTag("Header_cps").GetComponent<Animator>().SetTrigger(Bounce);

        store.Buy();
        UpdateBuildUI();
        return true;
    }

    public bool BuyUpgrade(Building building)
    {
        if (!EnoughForUpgradeBuilding(buildings.IndexOf(building)))
            return false;

        Coins = BigInteger.Subtract(Coins, building.CurrentUpgradeCost);
        GameObject.FindGameObjectWithTag("Header_lps").GetComponent<ColorFade>().FadeToColor(Color.white,
            utilies.HexToColor("#FF5C26"), typeof(TextMeshProUGUI));
        GameObject.FindGameObjectWithTag("Header_lps").GetComponent<Animator>().SetTrigger(Bounce);

        building.Upgrade();
        return true;
    }

    public bool BuyUpgrade(Store store)
    {
        if (!EnoughForUpgradeStore(stores.IndexOf(store)))
            return false;

        Coins = BigInteger.Subtract(Coins, store.CurrentUpgradeCost);
        GameObject.FindGameObjectWithTag("Header_cps").GetComponent<ColorFade>().FadeToColor(Color.white,
            utilies.HexToColor("#FF5C26"), typeof(TextMeshProUGUI));
        GameObject.FindGameObjectWithTag("Header_cps").GetComponent<Animator>().SetTrigger(Bounce);

        store.Upgrade();
        return true;
    }

    public bool EnoughForBuilding(int index)
    {
        return BigInteger.Compare(Logs, buildings[index].CurrentCost) >= 0;
    }

    public bool EnoughForStore(int index)
    {
        return BigInteger.Compare(Coins, stores[index].CurrentCost) >= 0;
    }

    public bool EnoughForUpgradeBuilding(int index)
    {
        return BigInteger.Compare(Coins, buildings[index].CurrentUpgradeCost) >= 0;
    }

    public bool EnoughForUpgradeStore(int index)
    {
        return BigInteger.Compare(Coins, stores[index].CurrentUpgradeCost) >= 0;
    }
}