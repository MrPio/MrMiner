using TMPro;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public User user;
    public bool resetProgresses;

    private void Start()
    {
        user = !resetProgresses ? User.Load() : new User();
        InitializeUser();

        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
        {
            shopItem.GetComponent<ShopItem>().Start();
            shopItem.GetComponent<ShopItem>().TurnAvailability(true);
            shopItem.GetComponent<ShopItem>().TurnUpgradeModeIfNecessary();
            shopItem.GetComponent<ShopItem>().TurnUpgradeAvailability(true);
        }

        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItemCoin"))
        {
            shopItem.GetComponent<ShopItem>().Start();
            shopItem.GetComponent<ShopItem>().TurnAvailability(true);
            shopItem.GetComponent<ShopItem>().TurnUpgradeModeIfNecessary();
            shopItem.GetComponent<ShopItem>().TurnUpgradeAvailability(true);
        }

        user.UpdateUI();
        user.UpdateBuildUI();
    }

    private void InitializeUser()
    {
        user.ShopItemValueText = new();
        user.ShopItemPriceText = new();
        user.ShopItemCoinValueText = new();
        user.ShopItemCoinPriceText = new();
        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
        {
            var shopBase = shopItem.transform.Find("ShopItem_base");
            user.ShopItemValueText.Add(shopBase.Find("ShopItem_value").GetComponent<TextMeshProUGUI>());
            user.ShopItemPriceText.Add(shopBase.Find("ShopItem_price").GetComponent<TextMeshProUGUI>());
        }

        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItemCoin"))
        {
            var shopBase = shopItem.transform.Find("ShopItem_base");
            user.ShopItemCoinValueText.Add(shopBase.Find("ShopItem_value").GetComponent<TextMeshProUGUI>());
            user.ShopItemCoinPriceText.Add(shopBase.Find("ShopItem_price").GetComponent<TextMeshProUGUI>());
        }
    }
}