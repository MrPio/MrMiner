using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class  DataStorage : MonoBehaviour
    {
        public User user;

        private void Start()
        {
            var x = WoodBuildings.Buildings;
            //TODO -----------------> User = User.Load();
            user = null;
            if (user == null)
                user = new User();
            else
            {
                Debug.Log("User loaded from save data!");
            }

            user.CalculateLps();
            user.CalculateClickPower();

            foreach (var shopItem in GameObject.FindGameObjectsWithTag("ShopItem"))
            {
                shopItem.GetComponent<ShopItem>().TurnAvailability(true);
                shopItem.GetComponent<ShopItem>().TurnUpdateModeIfNecessary();
            }
        }
    }
