using TMPro;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public User user;

    private void Start()
    {
        //TODO -----------------> User = User.Load();
        user = null;
        if (user == null)
            InitializeUser();
        else
            Debug.Log("User loaded from save data!");

        user.CalculateLps();
        user.CalculateClickPower();

        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
        {
            shopItem.GetComponent<ShopItem>().TurnAvailability(true);
            shopItem.GetComponent<ShopItem>().TurnUpgradeModeIfNecessary();
        }
    }

    private void InitializeUser()
    {
        user = new User();
        foreach (var building in GameObject.FindGameObjectsWithTag("ShopItem"))
        {
            var shopBase = building.transform.Find("ShopItem_base");
            user.shopItemValueText.Add(shopBase.Find("ShopItem_value").GetComponent<TextMeshProUGUI>());
            user.shopItemPriceText.Add(shopBase.Find("ShopItem_price").GetComponent<TextMeshProUGUI>());
        }
    }
}