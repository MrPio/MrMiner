using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LpsCps : MonoBehaviour
{
    [Range(10, 120)] public int Fps;
    private float _startTime;

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
            GameObject.Find("DataStorage").GetComponent<DataStorage>().User.EarnLps(Fps);
        }
    }
}