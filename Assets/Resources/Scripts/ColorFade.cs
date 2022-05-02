using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorFade : MonoBehaviour
{
    public float duration = 1f;
    private Color _color = Color.clear;
    private Color _initialColor;
    private Component _component;
    private float _startTime;
    private bool _animationDone = true;
    private Type _type;

    void Start()
    {
    }

    void Update()
    {
        if (_animationDone)
            return;

        var t = (Time.time - _startTime) / duration;
        if (_type == typeof(Image))
            GetComponent<Image>().color = Color.Lerp(_initialColor, _color, t);
        else if (_type == typeof(TextMeshProUGUI))
            GetComponent<TextMeshProUGUI>().color = Color.Lerp(_initialColor, _color, t);

        if (t > 1)
            _animationDone = true;
    }

    public void FadeToColor(Color color, Type type)
    {
        _color = color;
        _type = type;
        if (type == typeof(Image))
            _initialColor = GetComponent<Image>().color;
        else if (type == typeof(TextMeshProUGUI))
            _initialColor = GetComponent<TextMeshProUGUI>().color;
        _startTime = Time.time;
        _animationDone = false;
    }
}