using System.Collections.Generic;
using UnityEngine;

public class LpsCps : MonoBehaviour
{
    [Range(10, 120)] public int fps;
    
    private float _startTime;
    private float _passedSecond ;
    private User _user;
    private readonly List<ShopItem> _shopItems = new();

    private void Start()
    {
        _user = GameObject.Find("DataStorage").GetComponent<DataStorage>().user;
        foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
            _shopItems.Add(shopItem.GetComponent<ShopItem>());

        _startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _startTime > 1f / fps)
        {
            _startTime = Time.time;
            _user.EarnLps(fps);

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