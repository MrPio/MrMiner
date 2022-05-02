using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class  DataStorage : MonoBehaviour
    {
        public User user;

        private void Start()
        {
            //TODO -----------------> User = User.Load();
            if (user == null)
                user = new User();
            else
            {
                Debug.Log("User loaded from save data!");
            }

            user.CalculateLps();
            user.CalculateClickPower();
        }
    }
