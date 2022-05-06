using TMPro;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public User user;
    public bool resetProgresses;

    private void Start()
    {
        user = null;
        if (!resetProgresses)
            user = User.Load();
        if (user == null)
            user = new User();
        else
            Debug.Log("User loaded from save data!");

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
        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
        {
            var shopBase = shopItem.transform.Find("ShopItem_base");
            user.ShopItemValueText.Add(shopBase.Find("ShopItem_value").GetComponent<TextMeshProUGUI>());
            user.ShopItemPriceText.Add(shopBase.Find("ShopItem_price").GetComponent<TextMeshProUGUI>());
        }

        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItemCoin"))
        {
            var shopBase = shopItem.transform.Find("ShopItem_base");
            user.ShopItemValueText.Add(shopBase.Find("ShopItem_value").GetComponent<TextMeshProUGUI>());
            user.ShopItemPriceText.Add(shopBase.Find("ShopItem_price").GetComponent<TextMeshProUGUI>());
        }
    }
}