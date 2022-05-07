using System.Collections.Generic;
using UnityEngine;

public class LpsCps : MonoBehaviour
{
    [Range(1, 120)] public int fps = 60;

    private float _startTime;
    private float _passedSecond;
    [SerializeField] public DataStorage dataStorage;
    private readonly List<ShopItem> _shopItems = new();

    private void Start()
    {
        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
            _shopItems.Add(shopItem.GetComponent<ShopItem>());
        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItemCoin"))
            _shopItems.Add(shopItem.GetComponent<ShopItem>());
        Debug.Log("_shopItems--->" + _shopItems.Count);
        _startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _startTime > 1f / fps)
        {
            _startTime = Time.time;
            dataStorage.user.EarnLps(fps);
            dataStorage.user.EarnCps(fps);

            _passedSecond += 1f / fps;
            if (_passedSecond >= 0.5f)
            {
                _passedSecond -= 0.5f;
                foreach (var shopItem in _shopItems)
                {
                    shopItem.TurnAvailability();
                    shopItem.TurnUpgradeAvailability();
                }
            }
        }
    }
}