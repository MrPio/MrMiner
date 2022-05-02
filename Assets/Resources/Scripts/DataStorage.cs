using System;
using UnityEngine;
using UnityEngine.UIElements;

public class  DataStorage : MonoBehaviour
    {
        public User User;

        private void Start()
        {
            User = User.Load();
            if (User == null)
                User = new User();
            else
            {
                Debug.Log("User loaded from save data!");
            }

            User.CalculateLps();
            User.CalculateClickPower();
        }
    }
