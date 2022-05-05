using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ClickSpeed : MonoBehaviour
{
    public AnimationCurve timeToScale;
    public TextMeshProUGUI text;
    public Tree tree;

    private float _time;

    private void Update()
    {
        _time += Time.deltaTime;
        var time = tree.clickCurrentSpeed > 0 ? tree.clickCurrentSpeed : 300f;
        time *= 0.1f / 1000f;
        if (_time < time)
            return;
        var scale = timeToScale.Evaluate(time /0.1f);
        text.text = tree.clickCurrentSpeed.ToString(CultureInfo.InvariantCulture) + " cps";
        text.fontSize = scale;
        _time -= time;
    }
}