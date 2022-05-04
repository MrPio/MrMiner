using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class User
{
    private static readonly string SaveFileLocation = "/MrMiner_User.dat";

    public BigInteger Logs { get; private set; }
    public BigInteger Coins { get; }
    public DateTime ProfileCreated { get; }
    public DateTime LastAutosave { get; private set; }
    public BigInteger ClickPower { get; private set; }
    public List<Building> Buildings;
    public double Lps { get; private set; }
    public double Cps { get; private set; }
    public List<TextMeshProUGUI> shopItemValueText, shopItemPriceText;

    private int _clickVersionLog, _clickVersionCoin;
    private int _lpsPerc, _cpsPerc;
    private double _lpsRest, _cpsRest;
    private static readonly int Bounce = Animator.StringToHash("Bounce");


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
        shopItemValueText = new List<TextMeshProUGUI>();
        shopItemPriceText = new List<TextMeshProUGUI>();
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
        ClickPower = BigInteger.Add(ClickPower, new BigInteger(1000));
    }

    public void EarnLps(int fps)
    {
        Logs = BigInteger.Add(Logs, new BigInteger(Lps / fps + _lpsRest));
        _lpsRest += Lps / fps - Math.Truncate(Lps / fps);
        _lpsRest -= Math.Truncate(_lpsRest);
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
            utilies.NumToStr(Logs);
        GameObject.Find("Header_lps").GetComponent<TextMeshProUGUI>().text =
            utilies.DoubleToStr(Lps) + " lps";

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
            GameObject.Find("Header_lps").GetComponent<Animator>().SetTrigger(Bounce);

            building.Buy();
            CalculateLps();

            shopItemValueText.ElementAt(Buildings.IndexOf(building)).text = building.Count.ToString();
            shopItemPriceText.ElementAt(Buildings.IndexOf(building)).text = utilies.NumToStr(building.CurrentCost);
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
            GameObject.Find("Header_lps").GetComponent<Animator>().SetTrigger(Bounce);

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