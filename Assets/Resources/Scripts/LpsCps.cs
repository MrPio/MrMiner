using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LpsCps : MonoBehaviour
{
    [Range(10, 120)] public int Fps;
    private float _startTime;
    private float passedSecond = 0;

    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _startTime > 1f / Fps)
        {
            _startTime = Time.time;
            GameObject.Find("DataStorage").GetComponent<DataStorage>().user.EarnLps(Fps);
            
            passedSecond += 1f / Fps;
            if (passedSecond >= 0.5f)
            {
                passedSecond -= 0.5f;
                foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
                    shopItem.GetComponent<ShopItem>().TurnAvailability();
            }
        }
    }
}