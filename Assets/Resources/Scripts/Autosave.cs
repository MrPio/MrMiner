using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autosave : MonoBehaviour
{
    [Range(1,600)]
    [SerializeField] public int IntervalSeconds = 4;

    private float _startTime;
    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time-_startTime > IntervalSeconds)
        {
            _startTime = Time.time;
            GameObject.Find("DataStorage").GetComponent<DataStorage>().User.Save();
        }
    }
}
