using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using Debug = UnityEngine.Debug;

[Serializable]
public class User
{
    private static readonly string _saveFileLocation = "/MrMiner_User.dat";
    public BigInteger Logs { get; private set; }
    public BigInteger Coins { get; }
    public DateTime ProfileCreated { get; }
    public DateTime LastAutosave { get; private set; }
    public BigInteger ClickPower { get; private set; }
    public List<Building> Buildings;
    public double Lps { get; private set; }
    public double Cps { get; private set; }

    private int _clickVersionLog, _clickVersionCoin, _lpsPerc, _cpsPerc;


    public User()
    {
        Logs = new BigInteger(0);
        Coins = new BigInteger(0);
        ProfileCreated = DateTime.Now;
        Lps = 0;
        Cps = 0;
        ClickPower = 1;
        _lpsPerc = 0;
        _cpsPerc = 0;
        Buildings = new List<Building>();
        foreach (var building in WoodBuildings.Buildings)
            Buildings.Add(building);
        _clickVersionLog = 0;
        _clickVersionCoin = 0;
    }

    public void Save()
    {
        LastAutosave = DateTime.Now;
        var bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + _saveFileLocation))
            File.Delete(Application.persistentDataPath + _saveFileLocation);
        var file = File.Create(Application.persistentDataPath + _saveFileLocation);
        bf.Serialize(file, this);
        file.Close();
        Debug.Log("Game data saved!");
        Debug.Log(Load());
    }

    public static User Load()
    {
        if (!File.Exists(Application.persistentDataPath + _saveFileLocation))
            return null;
        var bf = new BinaryFormatter();
        var file =
            File.Open(Application.persistentDataPath + _saveFileLocation, FileMode.Open);
        var user = (User) bf.Deserialize(file);
        file.Close();
        user.UpdateUI();
        return user;
    }

    public void CalculateLps()
    {
        Lps = 0;
        foreach (var building in Buildings)
            Lps += building.CalculateLps();
        UpdateUI();
    }

    public void CalculateClickPower()
    {
        CalculateLps();
        ClickPower = BigInteger.Multiply(BigInteger.Pow(new BigInteger(2), _clickVersionLog),
            new BigInteger(Lps * _lpsPerc));
        //TODO
        ClickPower = BigInteger.Add(ClickPower, new BigInteger(1000));
    }

    public void EarnLps(int fps)
    {
        Logs = BigInteger.Add(Logs, new BigInteger(Lps / fps));
        UpdateUI();
    }

    public void EarnClick()
    {
        Logs = BigInteger.Add(Logs, ClickPower);
        UpdateUI();
    }

    public void EarnClick(BigInteger value)
    {
        Logs = BigInteger.Add(Logs, value);
        UpdateUI();
    }

    private void UpdateUI()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        GameObject.Find("Header_log_value").GetComponent<TextMeshProUGUI>().text =
            utilies.NumberToFormattedString(Logs);
        GameObject.Find("Header_lps").GetComponent<TextMeshProUGUI>().text =
            utilies.DoubleToFormattedString(Lps) + " lps";

        stopWatch.Stop();
        //Debug.Log("UI updated in: " + stopWatch.ElapsedTicks + "ticks =1/10 nano");
    }

    public bool Buy(Building building)
    {
        if (Logs >= building.CurrentCost)
        {
            Logs = BigInteger.Subtract(Logs, building.CurrentCost);
            GameObject.Find("Header_lps").GetComponent<ColorFade>().FadeToColor(Color.white,
                utilies.HexToColor("#FF5C26"), typeof(TextMeshProUGUI));
            GameObject.Find("Header_lps").GetComponent<Animator>().SetTrigger("Bounce");

            building.Buy();
            CalculateLps();

            var shopBase = GameObject.FindGameObjectsWithTag("ShopItem")[Buildings.IndexOf(building)].transform
                .Find("ShopItem_base");
            shopBase.Find("ShopItem_value").GetComponent<TextMeshProUGUI>().text = building.Count.ToString();
            shopBase.Find("ShopItem_price").GetComponent<TextMeshProUGUI>().text =
                utilies.NumberToFormattedString(building.CurrentCost);
            return true;
        }

        return false;
    }
    
    public bool BuyUpgrade(Building building)
    {
        if (building.CheckForUpgrade() && Logs >= building.CalculateUpgradeCost())
        {
            Logs = BigInteger.Subtract(Logs, building.CalculateUpgradeCost());
            GameObject.Find("Header_lps").GetComponent<ColorFade>().FadeToColor(Color.white,
                utilies.HexToColor("#FF5C26"), typeof(TextMeshProUGUI));
            GameObject.Find("Header_lps").GetComponent<Animator>().SetTrigger("Bounce");

            building.Upgrade();
            CalculateLps();

            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return $"{nameof(_clickVersionLog)}: {_clickVersionLog}, {nameof(_clickVersionCoin)}: {_clickVersionCoin}," +
               $" {nameof(_lpsPerc)}: {_lpsPerc}, {nameof(_cpsPerc)}: {_cpsPerc}, {nameof(Logs)}: {Logs}," +
               $" {nameof(Coins)}: {Coins}, {nameof(ProfileCreated)}: {ProfileCreated}, {nameof(LastAutosave)}: {LastAutosave}," +
               $" {nameof(ClickPower)}: {ClickPower}, {nameof(Lps)}: {Lps}, {nameof(Cps)}: {Cps}";
    }
}