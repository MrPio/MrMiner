using UnityEngine;

public class Autosave : MonoBehaviour
{
    [Range(1, 600)] public int intervalSeconds = 4;

    private User _user;
    private float _startTime;

    private void Start()
    {
        _user = GameObject.Find("DataStorage").GetComponent<DataStorage>().user;
        _startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _startTime > intervalSeconds)
        {
            _startTime = Time.time;
            _user.Save();
        }
    }
}